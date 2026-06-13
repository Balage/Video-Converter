using VideoConverter.Video;

namespace VideoConverter.Config
{
    public class AppSettingsData
    {
        public int Tab { get; set; } = 1;
        public int Codec { get; set; } = 1;
        public bool Downscale4K { get; set; } = false;
        public bool SkipIfSameCodec { get; set; } = false;
        public int QualityCRF { get; set; } = 23;
        public double FixedSize { get; set; } = 10.0;
        public int QualityMode { get; set; } = 0;
        public int GenerateFrames { get; set; } = 0;
        public int FrameRate { get; set; } = 30;
        public string BackgroundColor { get; set; } = "#ffffff";

        public int[] WindowPosition { get; set; } = [];
        public int[] WindowSize { get; set; } = [];
    }

    public class AppSettings : ConfigHandler<AppSettingsData>
    {
        public AppSettings() : base("settings.json") { }

        public TaskTab Tab
        {
            get => (TaskTab)Math.Clamp(_data.Tab, 0, 2);
            set => _data.Tab = (int)value;
        }

        public CodecType Codec
        {
            get => (CodecType)Math.Clamp(_data.Codec, 0, 1);
            set => _data.Codec = (int)value;
        }

        public bool Downscale4K
        {
            get => _data.Downscale4K;
            set => _data.Downscale4K = value;
        }

        public bool SkipIfSameCodec
        {
            get => _data.SkipIfSameCodec;
            set => _data.SkipIfSameCodec = value;
        }

        public int QualityCRF
        {
            get => Math.Clamp(_data.QualityCRF, 0, 51);
            set => _data.QualityCRF = value;
        }

        public double FixedSize
        {
            get => Math.Clamp(_data.FixedSize, 0.01, 2000.0);
            set => _data.FixedSize = value;
        }

        public QualityMode QualityMode
        {
            get => (QualityMode)Math.Clamp(_data.QualityMode, 0, 1);
            set => _data.QualityMode = (int)value;
        }

        public int GenerateFrames
        {
            get => Math.Clamp(_data.GenerateFrames, 0, 3);
            set => _data.GenerateFrames = value;
        }

        public int FrameRate
        {
            get => Math.Clamp(_data.FrameRate, 12, 240);
            set => _data.FrameRate = value;
        }

        public Color BackgroundColor
        {
            get => Utils.MathExt.ParseHexColor(_data.BackgroundColor);
            set => _data.BackgroundColor = Utils.MathExt.ColorToHex(value);
        }

        public Point? WindowPosition
        {
            get => _data.WindowPosition.Length == 2 ? new(_data.WindowPosition[0], _data.WindowPosition[1]) : null;
            set => _data.WindowPosition = value != null ? [value.Value.X, value.Value.Y] : [];
        }

        public Size WindowSize
        {
            get => _data.WindowSize.Length == 2
                ? new(
                    Math.Min(3840, _data.WindowSize[0]),
                    Math.Min(2160, _data.WindowSize[1])
                )
                : new(900, 573);
            set => _data.WindowSize = [value.Width, value.Height];
        }
    }
}