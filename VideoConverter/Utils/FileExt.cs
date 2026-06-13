namespace VideoConverter.Utils
{
    public static class FileExt
    {
        public static string AddSuffix(string path, string suffix, string? extension = null)
        {
            return Path.Combine(
                Path.GetDirectoryName(path) ?? "",
                $"{Path.GetFileNameWithoutExtension(path)}{suffix}{extension ?? Path.GetExtension(path)}"
            );
        }

        public static string ChangeExtension(string path, string newExt)
        {
            return Path.Combine(
                Path.GetDirectoryName(path) ?? "",
                $"{Path.GetFileNameWithoutExtension(path)}{newExt}"
            );
        }

        // Keeps appending " (#)" to the end of the file until a free name is found.
        public static string GetFreeFileName(string fileName)
        {
            if (File.Exists(fileName))
            {
                string start = Path.Combine(Path.GetDirectoryName(fileName) ?? "", Path.GetFileNameWithoutExtension(fileName));
                string ext = Path.GetExtension(fileName);
                int index = 1;
                do
                {
                    fileName = $"{start} ({index++}){ext}";
                }
                while (File.Exists(fileName));
            }
            return fileName;
        }
    }
}
