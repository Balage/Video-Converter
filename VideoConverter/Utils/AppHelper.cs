using System.Diagnostics;
using System.Reflection;

namespace VideoConverter.Utils
{
    public static class AppHelper
    {
        public static string GetAssemblyVersion() => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "-";

        public static void OpenWebsite(string url)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        public static void RunWithMutex(Form form, string errorText, string errorTitle)
        {
            var mutexId = $"Global\\vbstudio_hu_{Assembly.GetExecutingAssembly().GetName().Name}";

            using (var mutex = new Mutex(false, mutexId, out _))
            {
                var hasHandle = false;
                try
                {
                    try
                    {
                        hasHandle = mutex.WaitOne(0, false);
                        if (hasHandle == false)
                        {
                            MessageBox.Show(errorText, errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }
                    catch (AbandonedMutexException)
                    {
                        // Mutex was abandoned in another process, but it will still get acquired.
                        hasHandle = true;
                    }

                    Application.Run(form);
                }
                finally
                {
                    if (hasHandle)
                    {
                        mutex.ReleaseMutex();
                    }
                }
            }
        }
    }
}
