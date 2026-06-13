using System.Globalization;

namespace VideoConverter.Video
{
    public enum QualityMode
    {
        FixedQuality,
        FixedFileSize,
    }

    public struct ProcessorArgs
    {
        public TaskTab TaskTab;
        public CodecType Codec;
        public bool SkipForSameCodec;
        public bool Downscale4K;
        public bool DropAudio;
        public QualityMode QualityMode;
        public int ConstantRateFactor;
        public double FixedSizeMB;
        public int GenFrameCount;
        public bool GenFrameLoop;
        public int FrameRate;
        public string BackgroundColor;
    }

    public struct ProgressInfo(double taskProgress, int taskIndex = 0, int totalTasks = 1)
    {
        public double TaskProgress = taskProgress; // %
        public int TaskIndex = taskIndex;
        public int TotalTasks = totalTasks;
    }

    public class Processor
    {
        // Const
        private const double ByteToMega = 1024.0 * 1024.0;

        // Public
        public Action? OnCompleted { get; set; } = null;

        // State
        private readonly ProcessorArgs _args;
        private readonly Action<string, PrintType> _printCallback;
        private readonly Action<ProgressInfo> _progressCallback;
        private readonly string _tempFolder;
        private FFMpegTask? _task;

        // Private fn
        private void Print(string text, PrintType type = PrintType.Normal) => _printCallback(text, type);

        public Processor(
            ProcessorArgs args,
            Action<string, PrintType> printCallback,
            Action<ProgressInfo> progressCallback,
            string tempFolder)
        {
            _args = args;
            _printCallback = printCallback;
            _progressCallback = progressCallback;
            _tempFolder = tempFolder;
        }

        public void Process(string[] files)
        {
            if (files.Length > 0)
            {
                switch (_args.TaskTab)
                {
                    case TaskTab.Probe:
                    {
                        for (int i = 0; i < files.Length; i++)
                        {
                            string fileName = files[i];
                            Print($"File #{i + 1}: \"{Path.GetFileName(fileName)}\"");

                            // Get and print source video info
                            var info = new FFProbeResult(fileName);
                            info.Print((text) => Print($"  {text}"));
                        }
                        break;
                    }
                    case TaskTab.ConvertVideo:
                    {
                        for (int i = 0; i < files.Length; i++)
                        {
                            string fileName = files[i];
                            Print($"File {i + 1}/{files.Length}: \"{fileName}\"");
                            if (ConvertVideoFile(fileName, i, files.Length))
                            {
                                break;
                            }
                        }
                        break;
                    }
                    case TaskTab.ImagesToVideo:
                    {
                        ConvertImagesToVideo(files);
                        break;
                    }
                }
            }
            Print("  Done.");
            OnCompleted?.Invoke();
        }

        private void ConvertImagesToVideo(string[] files)
        {
            Print($"Converting {files.Length} images to video at {_args.FrameRate} FPS.");

            var firstFrame = new Bitmap(files[0]); // TODO: ffprobe could do this too.
            if (firstFrame == null)
            {
                Print("  Failed to open first file.");
                return;
            }

            // Looping and frame interpolation: add first frame as last too.
            //   This is only for the interpolation, and the keyframe will be cropped.
            /*if (_args.GenFrameCount > 0 && _args.GenFrameLoop)
            {

                string tempLastFrame = Path.Combine(Path.GetDirectoryName(files[0]) ?? "", "temp.png");
                File.Copy(files[0], tempLastFrame, true);
                files = [.. files, tempLastFrame];
            }*/

            int fpsMul = _args.GenFrameCount + 1;
            long totalFrames = files.Length * fpsMul - _args.GenFrameCount;

            string targetFile = Utils.FileExt.ChangeExtension(files[0], ".mp4");
            bool statusPrintStarted = false;
            _task = new FFMpegTask(files, _args.FrameRate, targetFile)
            {
                TempFolder = _tempFolder,
                PrintCallback = Print,
                StatusCallback = (status) =>
                {
                    double percent = status.Frame / (double)totalFrames;
                    _progressCallback?.Invoke(new(percent));
                    Print(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "  Converting {0:0}%  (speed: {1:0.0#}x)",
                            percent * 100.0,
                            status.Speed
                        ),
                        statusPrintStarted ? PrintType.NormalOverwriteLast : PrintType.Normal
                    );
                    statusPrintStarted = true;
                }
            };

            // Filter
            // Add white background to transparent PNG.
            // FPS needs to be emphasized, because otherwise the filter makes the source stream to be 25 FPS for some reason.
            string filterComplex = $"color={_args.BackgroundColor}:{firstFrame.Width}x{firstFrame.Height},fps={_args.FrameRate}[c]; [c][0:v]overlay=shortest=1";

            // Generate in-between frames.
            if (_args.GenFrameCount > 0)
            {
                int targetFps = _args.FrameRate * fpsMul;
                Print($"  Generating {_args.GenFrameCount} in-between frames for a final {targetFps} FPS.");

                // Interpolated frames
                filterComplex = $"{filterComplex},minterpolate=fps={targetFps}:me_mode=bidir";
                //filterComplex = $"{backgroundFilter},minterpolate=fps={targetFps}:me_mode=bidir,tblend=all_mode=average,framestep={fpsMul}"

                // Looping
                if (_args.GenFrameLoop) // TODO: Figure out how to make this work
                {
                    //filterComplex = $"{filterComplex},select=between(n\\,0\\,{totalFrames - 2}),setpts=PTS-STARTPTS";
                    //filterComplex = $"{filterComplex},trim=end_frame={totalFrames - 2}";
                }
            }
            _task.AddArg("filter_complex", $"\"{filterComplex}\"");

            SetCodec(_task);
            SetConstantRateFactor(_task); // TODO: Signal somehow that fixed size is not available for this
            _task.AddArg("pix_fmt", "yuv420p");
            _task.AddArg("f", "mp4");
            _task.RunTask(_args.FrameRate);
            _task = null;
        }

        // Returns true if it was terminated
        private bool ConvertVideoFile(string fileName, int fileIndex, int totalCount)
        {
            // Get and print source video info
            var info = new FFProbeResult(fileName);
            info.Print((text) => Print(text, PrintType.Verbose));

            // Skip file if source and target codec are the same
            if (_args.SkipForSameCodec &&
                info.GetCodec() == _args.Codec)
            {
                Print($"  Video is already in {GetCodecTypeString(_args.Codec)} format, skipping.");
                return false;
            }

            bool wasTerminated = false;
            switch (_args.QualityMode)
            {
                case QualityMode.FixedQuality:
                {
                    long? originalSize = info.GetFormatLong("size");
                    double totalTime = info.GetFormatDouble("duration") ?? 0.0;
                    //double totalFrames = info.GetVideoLong("nb_frames") ?? 1;

                    bool statusPrintStarted = false;
                    _task = new FFMpegTask(fileName, GetTargetFileSuffix(_args.Codec))
                    {
                        TempFolder = _tempFolder,
                        PrintCallback = Print,
                        StatusCallback = (status) =>
                        {
                            double percent = status.Time / totalTime;
                            _progressCallback?.Invoke(new(percent, fileIndex, totalCount));
                            Print(
                                string.Format(
                                    CultureInfo.InvariantCulture,
                                    "  Converting {0:0}%  (speed: {1:0.0#}x)",
                                    percent * 100.0,
                                    status.Speed
                                ),
                                statusPrintStarted ? PrintType.NormalOverwriteLast : PrintType.Normal
                            );
                            statusPrintStarted = true;
                        }
                    };

                    SetCodec(_task);
                    SetConstantRateFactor(_task);
                    SetScale(_task, info);
                    SetDropAudio(_task);
                    _task.RunTask();
                    ResultFileSizeCheck(_task.OutputFile, originalSize ?? 0);
                    wasTerminated = _task.WasTerminated;
                    _task = null;
                    break;
                }
                case QualityMode.FixedFileSize:
                {
                    long? originalSize = info.GetFormatLong("size");
                    double? originalDuration = info.GetFormatDouble("duration");
                    if (!originalSize.HasValue ||
                        !originalDuration.HasValue)
                    {
                        Print("  Invalid source format.");
                        return false;
                    }

                    bool useAudio = info.HasAudio && !_args.DropAudio;
                    int? originalBitRate = (int?)info.GetAudioLong("bit_rate");

                    double videoRate;
                    if (useAudio)
                    {
                        if (!originalBitRate.HasValue)
                        {
                            Print("  Invalid source format.");
                            return false;
                        }

                        originalBitRate /= 1024; // Original audio rate in KiB/s

                        // Calculate target video rate - MB -> KiB/s
                        videoRate = (_args.FixedSizeMB * 8192.0) / originalDuration.Value - originalBitRate.Value;

                        // Check file size
                        double audioSize = originalBitRate.Value / 8192.0 * originalDuration.Value;
                        Print(string.Format(
                            CultureInfo.InvariantCulture,
                            "  Audio stream size (fix): {0:0.###} MB",
                            audioSize
                        ));

                        double videoTargetSize = videoRate / 8192.0 * originalDuration.Value;
                        if (videoTargetSize < 0.03) // Min stream size
                        {
                            Print($"  Target file size is smaller than the entire audio stream! Choose a bigger size.");
                            return false;
                        }
                        Print(string.Format(
                            CultureInfo.InvariantCulture,
                            "  Target video size: {0:0.###} MB",
                            videoTargetSize
                        ));
                    }
                    else
                    {
                        // Calculate target video rate - MB -> KiB/s
                        videoRate = (_args.FixedSizeMB * 8192.0) / originalDuration.Value;
                        Print(string.Format(
                            CultureInfo.InvariantCulture,
                            "  Target video size: {0:0.###} MB",
                            _args.FixedSizeMB
                        ));
                        Print(!info.HasAudio ? "  Video had no audio track" : "  Dropping audio track.");
                    }

                    double totalTime = info.GetFormatDouble("duration") ?? 0.0;
                    //double totalFrames = info.GetVideoLong("nb_frames") ?? 1;

                    // PASS 1
                    {
                        bool statusPrintStarted = false;
                        _task = new FFMpegTask(fileName)
                        {
                            TempFolder = _tempFolder,
                            PrintCallback = Print,
                            StatusCallback = (status) =>
                            {
                                double percent = status.Time / totalTime;
                                _progressCallback?.Invoke(new(percent * 0.5f, fileIndex, totalCount));
                                Print(
                                    string.Format(
                                        CultureInfo.InvariantCulture,
                                        "  Pass #1 at {00:0}%  (speed: {1:0.0#}x)",
                                        percent * 100.0,
                                        status.Speed
                                    ),
                                    statusPrintStarted ? PrintType.NormalOverwriteLast : PrintType.Normal
                                );
                                statusPrintStarted = true;
                            }
                        };

                        SetCodec(_task);
                        SetScale(_task, info);
                        _task.AddArg("b:v", $"{(long)videoRate}k");
                        _task.AddArg("pass", "1");
                        _task.AddArg("an");
                        _task.AddArg("f", "mp4");
                        _task.RunTaskOutputNull();
                        wasTerminated = _task.WasTerminated;
                        _task = null;
                    }

                    // PASS 2
                    if (!wasTerminated)
                    {
                        bool statusPrintStarted = false;
                        _task = new FFMpegTask(fileName, GetTargetFileSuffix(_args.Codec))
                        {
                            TempFolder = _tempFolder,
                            PrintCallback = Print,
                            StatusCallback = (status) =>
                            {
                                double percent = status.Time / totalTime;
                                _progressCallback?.Invoke(new(percent * 0.5f + 0.5f, fileIndex, totalCount));
                                Print(
                                    string.Format(
                                        CultureInfo.InvariantCulture,
                                        "  Pass #2 at {0:0}%  (speed: {1:0.0#}x)",
                                        percent * 100.0,
                                        status.Speed
                                    ),
                                    statusPrintStarted ? PrintType.NormalOverwriteLast : PrintType.Normal
                                );
                                statusPrintStarted = true;
                            }
                        };

                        SetCodec(_task);
                        SetScale(_task, info);
                        if (useAudio)
                        {
                            _task.AddArg("c:a", "copy");
                        }
                        else
                        {
                            _task.AddArg("an");
                        }
                        _task.AddArg("b:v", $"{(long)videoRate}k");
                        _task.AddArg("pass", "2");
                        _task.AddArg("c:a", "copy");
                        _task.RunTask();
                        ResultFileSizeCheck(_task.OutputFile, originalSize.Value);
                        wasTerminated = _task.WasTerminated;
                        _task = null;
                    }
                    break;
                }
            }
            return wasTerminated;
        }

        // Returns true if task was stopped.
        public bool Stop() => _task?.Stop() ?? false;

        private void ResultFileSizeCheck(string outputFile, long originalSize)
        {
            double originalSizeMB = originalSize / ByteToMega;
            double outputSizeMB = new FileInfo(outputFile).Length / ByteToMega;
            if (originalSizeMB < outputSizeMB)
            {
                Print(string.Format(
                    CultureInfo.InvariantCulture,
                    "  NOTE: New file is larger than original! ({0:0.##} MB → {1:0.##} MB)",
                    originalSizeMB,
                    outputSizeMB
                ));
            }
            else
            {
                Print(string.Format(
                    CultureInfo.InvariantCulture,
                    "  New size: {0:0.##} MB → {1:0.##} MB, {2:0.#}% of original.",
                    originalSizeMB,
                    outputSizeMB,
                    outputSizeMB / originalSizeMB * 100
                ));
            }
        }


        //
        // PRIVATE FN
        //
        private void SetCodec(FFMpegTask task)
        {
            switch (_args.Codec)
            {
                case CodecType.H265:
                {
                    task.AddArg("c:v", "libx265");
                    task.AddArg("vtag", "hvc1");

                    // Hardware acceleration has reduced quality, and thus only suitable for streaming!
                    //task.AddArg("vcodec", "hevc_nvenc"); // Enable hardware encoding
                    break;
                }
                default:
                {
                    task.AddArg("c:v", "libx264");
                    break;
                }
            }
        }

        private void SetConstantRateFactor(FFMpegTask task)
        {
            task.AddArg("crf", _args.ConstantRateFactor.ToString());
        }

        private void SetScale(FFMpegTask task, FFProbeResult info)
        {
            if (_args.Downscale4K)
            {
                // Resize to 1080p, but only if size is exactly 4K
                var size = info.GetVideoSize();
                if (size.Width == 3840 && size.Height == 2160)
                {
                    task.AddArg("vf", "scale=1920:1080");
                    Print("  Converting 4K to 1080p.");
                }
                else if (size.Width == 2160 && size.Height == 3840)
                {
                    task.AddArg("vf", "scale=1080:1920"); // Portrait
                    Print("  Converting 4K to 1080p.");
                }
            }
        }

        private void SetDropAudio(FFMpegTask task)
        {
            if (_args.DropAudio)
            {
                task.AddArg("an");
                Print("  Dropping audio track.");
            }
        }


        //
        // STATIC FN
        //
        private static string GetCodecTypeString(CodecType type) => type switch
        {
            CodecType.H264 => "H.264",
            CodecType.H265 => "H.265",
            _ => "[other]",
        };

        private static string GetTargetFileSuffix(CodecType type) => type switch
        {
            CodecType.H264 => " (x264)",
            CodecType.H265 => " (x265)",
            _ => " (other)", // This is error, should not happen
        };
    }
}
