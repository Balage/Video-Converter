//#pragma warning disable WFO5001

using VideoConverter.Utils;
using VideoConverter.Video;

namespace VideoConverter
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            //Application.SetColorMode(SystemColorMode.System);

            // Check executables
            if (!FFMpegTask.FileExists() || !FFProbeResult.FileExists())
            {
                string text = "'ffmpeg.exe' and 'ffprobe.exe' are missing!\nTo use this GUI, first download ffmpeg binaries, and place them in the same folder as this executable!\n\nWould you like to visit the download website now?";
                if (MessageBox.Show(text, "Missing ffmpeg", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    AppHelper.OpenWebsite("https://www.ffmpeg.org/download.html");
                }
                Application.Exit();
                return;
            }

            // Start app
            AppHelper.RunWithMutex(
                new MainForm(),
                "Another instance of this application is already running!",
                "Video Converter"
            );

            // NOTE: Use this instead if RunWithMutex is causing errors. That's only for preventing multiple instances to run at the same time.
            //Application.Run(new MainForm()); 
        }
    }
}