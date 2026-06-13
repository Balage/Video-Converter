using System.Diagnostics;

namespace VideoConverter.Utils
{
    public class ShortcutHelper
    {
        private readonly string _shortcutPath;

        public bool Exists => File.Exists(_shortcutPath);

        public ShortcutHelper(string appName)
        {
            _shortcutPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.StartMenu),
                $"{appName}.lnk"
            );
        }

        public bool Create()
        {
            try
            {
                var shell = new IWshRuntimeLibrary.WshShell();
                var shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(_shortcutPath);
                shortcut.TargetPath = Application.ExecutablePath;
                shortcut.Save();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Delete()
        {
            try
            {
                File.Delete(_shortcutPath);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
