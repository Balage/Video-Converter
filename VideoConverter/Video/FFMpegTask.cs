using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace VideoConverter.Video
{
    public class FFMpegStatus(double time, long frame, double speed)
    {
        public double Time = time;
        public long Frame = frame;
        public double Speed = speed;
    }

    public class FFMpegTask
    {
        // Consts
        private const string FFMPEG_EXE = "ffmpeg.exe";

        // CTOR consts
        private readonly string[] _inputFile;
        public readonly string OutputFile;

        // Public
        public static bool FileExists() => File.Exists(FFMPEG_EXE);
        public Action<string, PrintType>? PrintCallback { get; set; } = null;
        public Action<FFMpegStatus>? StatusCallback { get; set; } = null;
        public string? TempFolder = null;

        // State
        private readonly List<string> _args = [];
        private readonly string? _tempListFile = null;
        private Process? _process;
        public bool WasTerminated { get; private set; } = false;

        // Regex
        // frame=  106 fps= 23 q=37.2 size=    1280KiB time=00:00:02.10 bitrate=4993.4kbits/s speed=0.449x elapsed=0:00:04.67 
        //private readonly Regex _stateRegex = new(@"^.*frame= *(?<frame>[0-9]+).*speed= *(?<speed>[0-9]+\.[0-9]+x).*$");
        private readonly Regex _stateRegex = new(@"^.*frame= *(?<frame>[0-9]+).*time= *(?<h>[0-9]+):(?<m>[0-9]+):(?<s>[0-9]+)\.(?<ms>[0-9]+).*speed= *(?<speed>[0-9]+\.[0-9]+x).*$");

        public FFMpegTask(string[] inputFiles, int frameRate, string outputFile) // For image sequence
        {
            if (inputFiles.Length == 0) throw new ArgumentOutOfRangeException(nameof(inputFiles));

            string duration = $"duration {(1.0 / frameRate).ToString(CultureInfo.InvariantCulture)}";
            _tempListFile = Path.GetTempFileName();

            File.WriteAllLines(_tempListFile, inputFiles.Select(x => $"file '{x.Replace("'", "\\'")}'\n{duration}"));

            _inputFile = inputFiles;
            OutputFile = Utils.FileExt.GetFreeFileName(outputFile);
        }

        public FFMpegTask(string inputFile, string? outputSuffix = null) // For single file conversion
        {
            if (string.IsNullOrWhiteSpace(inputFile)) throw new ArgumentNullException(nameof(inputFile));

            _inputFile = [inputFile];
            OutputFile = Utils.FileExt.GetFreeFileName(Utils.FileExt.AddSuffix(inputFile, outputSuffix ?? " (new)", ".mp4"));
        }

        public void AddArg(string name)
        {
            _args.Add($"-{name}");
        }

        public void AddArg(string name, string value)
        {
            _args.Add($"-{name} {value}");
        }

        private void Print(string text)
        {
            var regex = _stateRegex.Match(text);
            if (regex.Success)
            {
                int h = Utils.MathExt.ParseInt(regex.Groups["h"].Value, 0);
                int m = Utils.MathExt.ParseInt(regex.Groups["m"].Value, 0);
                int s = Utils.MathExt.ParseInt(regex.Groups["s"].Value, 0);
                int ms = Utils.MathExt.ParseInt(regex.Groups["ms"].Value, 0);
                double time = h * 3600 + m * 60 + s + ms / 100.0;

                StatusCallback?.Invoke(new(
                    time,
                    Utils.MathExt.ParseLong(regex.Groups["frame"].Value, 0L),
                    Utils.MathExt.ParseDouble(regex.Groups["speed"].Value.TrimEnd('x'), 0.0)
                ));
            }
            PrintCallback?.Invoke(text, PrintType.Verbose);
        }

        private string GetInputArg()
        {
            string result = _tempListFile == null
                ? $"-i \"{_inputFile[0]}\""
                : $"-f concat -safe 0 -i \"{_tempListFile}\"";

            if (TempFolder != null) result += $" -passlogfile \"{Path.Combine(TempFolder, "passlog_")}\"";
            return $"{result} -y";
        }

        public void RunTask()
        {
            Run($"{GetInputArg()} {string.Join(" ", _args)} \"{OutputFile}\"");
        }

        public void RunTask(int framerate)
        {
            Run($"-r {framerate} {GetInputArg()} {string.Join(" ", _args)} \"{OutputFile}\"");
        }

        public void RunTaskOutputNull()
        {
            Run($"{GetInputArg()} {string.Join(" ", _args)} NUL");
        }

        private int Run(string args)
        {
            Print("[FFMPEG]");
            Print($">> Args: {args}");
            if (_tempListFile != null)
            {
                Print($">> Temp list file: {_tempListFile}");
            }

            var procInfo = new ProcessStartInfo()
            {
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                ErrorDialog = false,
                WorkingDirectory = Path.GetDirectoryName(FFMPEG_EXE),
                FileName = FFMPEG_EXE,
                Arguments = args
            };

            WasTerminated = false;
            _process = new Process
            {
                StartInfo = procInfo
            };

            _process.OutputDataReceived += (o, e) => Print($"    {e.Data}");
            _process.ErrorDataReceived += (o, e) => Print($"    {e.Data}");

            try
            {
                _process.Start();
                if (_process == null)
                {
                    Print(">> Failed to start process.");
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Print($">> Process failed: {ex.Message}");
                return -1;
            }

            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();
            _process.WaitForExit();

            Print(WasTerminated
                ? $">> Process was terminated by user. Exit code: {_process.ExitCode}."
                : $">> Process ended with {_process.ExitCode}.");

            Print("[/FFMPEG]");

            if (_tempListFile != null)
            {
                File.Delete(_tempListFile);
            }

            int exitCode = _process.ExitCode;
            _process = null;
            return exitCode;
        }

        public bool Stop()
        {
            if (_process != null)
            {
                _process.StandardInput.WriteLine("q");
                WasTerminated = true;
                return true;
            }
            return false;
        }
    }
}
