namespace WC3Files.Strings
{
    internal class WtsReader : IDisposable
    {
        //Данные
        MemoryStream Stream;
        private StreamReader Reader;

        //Конструктор
        public WtsReader(byte[] data)
        {
            Stream = new MemoryStream(data);
            Reader = new StreamReader(Stream);
        }
        public void Dispose()
        {
            Reader.Dispose();
            Stream.Dispose();
        }

        //Парсинг
        public WtsFile ReadFile()
        {
            //Данные
            WtsFile f = new WtsFile();

            //Файл
            bool bracers = false;
            string name = "";
            string value = "";
            while (Reader.ReadLine() is string lineSrc)
            {
                //Строка
                string line = lineSrc.Trim();
                if (string.IsNullOrEmpty(line)) continue;

                //Комментарий
                if (line.StartsWith("//"))
                {
                    //Пропуск
                }
                //Запись
                else if (line == "{")
                {
                    bracers = true;
                }
                else if (line == "}")
                {
                    bracers = false;
                    f.Dict[name] = value;
                    name = "";
                    value = "";
                }
                else
                {
                    if (bracers) { value += (value.Length>0)?"\n":"" + line; }
                    else { name = line; }
                }
            }
            return f;
        }
    }
}
