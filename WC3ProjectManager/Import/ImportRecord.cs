namespace WC3ProjectManager
{
    public class ImportRecord
    {
        /// <summary>
        /// Путь к файлу на диске
        /// </summary>
        public string DiscPath
        {
            set;
            get;
        } = "";

        /// <summary>
        /// Путь к файлу внутри карты
        /// </summary>
        public string ImportPath
        {
            set;
            get;
        } = "";

        /// <summary>
        /// Бинарные данные файла.
        /// </summary>
        public byte[] FileData
        {
            set;
            get;
        }

        //Сохранить в файл
        public void SaveToFile(string path)
        {
            string? dir = Path.GetDirectoryName(path);
            if (dir != null) Directory.CreateDirectory(dir);
            File.WriteAllBytes(path, FileData);
        }
    }
}