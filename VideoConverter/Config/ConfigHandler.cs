using System.Diagnostics;
using System.Text.Json;

namespace VideoConverter.Config
{
    public class ConfigHandler<TData> where TData : class, new()
    {
        private readonly JsonSerializerOptions SERIALIZER_OPTIONS = new()
        {
            WriteIndented = true
        };

        // Const
        private const string COMPANY_NAME = "VB studio";
        private const string APP_NAME = "FFMPEG Converter";

        // State
        private readonly string _fileName;
        protected TData _data = new();

        public ConfigHandler(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentOutOfRangeException(nameof(fileName));

            string folder = GetSaveFolder();
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            _fileName = Path.Combine(folder, fileName);
        }

        public string GetSaveFolder()
        {
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                COMPANY_NAME,
                APP_NAME
            );
#if DEBUG
            path += " (Debug)";
#endif
            return path;
        }

        public bool Load()
        {
            try
            {
                if (File.Exists(_fileName))
                {
                    string fileData = File.ReadAllText(_fileName);
                    var data = JsonSerializer.Deserialize<TData>(fileData);
                    if (data != null)
                    {
                        _data = data;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                _data = new(); // Reset
            }
            return false;
        }

        public bool Save()
        {
            try
            {
                string fileData = JsonSerializer.Serialize(_data ?? new TData(), SERIALIZER_OPTIONS);
                File.WriteAllText(_fileName, fileData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            return true;
        }
    }
}
