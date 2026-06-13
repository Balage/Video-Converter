using System.Diagnostics;

namespace VideoConverter.Video
{
    public enum CodecType
    {
        H264 = 0,
        H265 = 1,
        Other = 10,
    };

    public class FFProbeResult
    {
        private enum SectionType
        {
            None,
            Stream,
            Format,
            Unknown
        };

        // Consts
        private const string FFPROBE_EXE = "ffprobe.exe";

        // Public
        public static bool FileExists() => File.Exists(FFPROBE_EXE);
        public bool HasVideo => _videoStreamIndex != -1;
        public bool HasAudio => _audioStreamIndex != -1;

        // State
        private readonly Dictionary<string, string> _format = [];
        private readonly List<Dictionary<string, string>> _streams = [];
        private readonly int _videoStreamIndex = -1;
        private readonly int _audioStreamIndex = -1;

        public FFProbeResult(string fileName)
        {
            // -v error -- disable extra printed info
            string args = $"-v error -show_format -show_streams \"{fileName}\"";

            var procInfo = new ProcessStartInfo()
            {
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                //RedirectStandardInput = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                ErrorDialog = false,
                WorkingDirectory = Path.GetDirectoryName(FFPROBE_EXE),
                FileName = FFPROBE_EXE,
                Arguments = args
            };
            var process = new Process
            {
                StartInfo = procInfo
            };

            var section = SectionType.None;
            process.OutputDataReceived += (o, e) =>
            {
                string row = (e.Data ?? "").Trim();
                switch (section)
                {
                    case SectionType.None:
                    {
                        switch (row)
                        {
                            case "[FORMAT]": section = SectionType.Format; break;
                            case "[STREAM]":
                                section = SectionType.Stream;
                                _streams.Add([]);
                                break;
                        }
                        break;
                    }
                    case SectionType.Format:
                    {
                        if (row == "[/FORMAT]")
                        {
                            section = SectionType.None;
                        }
                        else
                        {
                            string[] keyValue = row.Split('=');
                            if (keyValue.Length == 2)
                            {
                                _format.Add(keyValue[0], keyValue[1]);
                            }
                            // else TODO: error!
                        }
                        break;
                    }
                    case SectionType.Stream:
                    {
                        if (row == "[/STREAM]")
                        {
                            section = SectionType.None;
                        }
                        else
                        {
                            string[] keyValue = row.Split('=');
                            if (keyValue.Length == 2)
                            {
                                _streams[_streams.Count - 1].Add(keyValue[0], keyValue[1]); break;
                            }
                            // else TODO: error!
                        }
                        break;
                    }
                }
            };

            try
            {
                process.Start();
                if (process == null)
                {
                    //Print("Failed to start process.");
                    return;
                }
            }
            catch (Exception)
            {
                //Print($">> Process failed: {ex.Message}");
                return;
            }

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            // Find first audio and video stream
            for (int i = 0; i < _streams.Count; i++)
            {
                foreach (var stream in _streams[i])
                {
                    if (stream.Key == "codec_type")
                    {
                        if (stream.Value == "video" && _videoStreamIndex == -1)
                        {
                            _videoStreamIndex = i;
                        }
                        if (stream.Value == "audio" && _audioStreamIndex == -1)
                        {
                            _audioStreamIndex = i;
                        }
                    }
                }
            }
        }


        //
        // GENERIC GETTERS
        //
        public string? GetFormat(string key)
        {
            return _format.TryGetValue(key, out var result) ? result : null;
        }
        public long? GetFormatLong(string key) => Utils.MathExt.ParseLong(GetFormat(key));
        public double? GetFormatDouble(string key) => Utils.MathExt.ParseDouble(GetFormat(key));

        public string? GetVideo(string key)
        {
            if (_videoStreamIndex >= 0)
            {
                return _streams[_videoStreamIndex].TryGetValue(key, out var result) ? result : null;
            }
            return null;
        }
        public long? GetVideoLong(string key) => Utils.MathExt.ParseLong(GetVideo(key));
        public double? GetVideoDouble(string key) => Utils.MathExt.ParseDouble(GetVideo(key));

        public string? GetAudio(string key)
        {
            if (_audioStreamIndex >= 0)
            {
                return _streams[_audioStreamIndex].TryGetValue(key, out var result) ? result : null;
            }
            return null;
        }
        public long? GetAudioLong(string key) => Utils.MathExt.ParseLong(GetAudio(key));
        public double? GetAudioDouble(string key) => Utils.MathExt.ParseDouble(GetAudio(key));


        //
        // SPECIAL GETTERS
        //
        public Size GetVideoSize()
        {
            return new Size(
                (int?)GetVideoLong("width") ?? 0,
                (int?)GetVideoLong("height") ?? 0
            );
        }

        public CodecType GetCodec()
        {
            string? codecName = GetVideo("codec_name");
            if (codecName != null)
            {
                if (codecName == "hevc") return CodecType.H265;
                if (codecName == "h264") return CodecType.H264;
            }
            return CodecType.Other;
        }


        //
        // DEBUG
        //
        public void Print(Action<string> printCallback)
        {
            printCallback("[FFPROBE]");
            printCallback("  [FORMAT]");
            foreach (var item in _format)
            {
                printCallback($"    {item.Key} = \"{item.Value}\"");
            }

            for (int i = 0; i < _streams.Count; i++)
            {
                printCallback($"  [STREAM #{i}]");
                foreach (var item in _streams[i])
                {
                    printCallback($"    {item.Key} = \"{item.Value}\"");
                }
            }
            printCallback("[/FFPROBE]");
        }
    }
}
