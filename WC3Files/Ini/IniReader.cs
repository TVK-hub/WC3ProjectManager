using WC3Files.Ini;
namespace WC3Files.Triggers
{
    public class IniReader
    {
        //Данные
        private IEnumerable<string> Lines;

        //Конструктор
        public IniReader(string path)
        {
            Lines = File.ReadLines(path);
        }

        //Парсинг
        public IniFile ReadFile()
        {
            //Данные
            IniFile f = new IniFile();

            //Файл
            IniSection section = new IniSection("");
            IniSection sectionGlb = section;
            foreach (string lineSrc in Lines)
            {
                //Строка
                string line = lineSrc.Trim();
                if (string.IsNullOrEmpty(line)) continue;

                //Секция
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    string name = line[1..^1];
                    section = new IniSection(name);
                    f.Sections.Add(section);
                }
                //Комментарий
                else if (line.StartsWith("//"))
                {
                    //Пропуск
                }
                //Запись
                else
                {
                    IniEntry en = new IniEntry(section);
                    int sep = line.IndexOf('=');
                    en.Name = line[..sep].Trim();
                    en.Value = line[(sep + 1)..].Trim();
                    section.Entries.Add(en);
                    f.Entries.Add(en);
                }
            }
            if (sectionGlb.Entries.Count > 0) { f.Sections.Add(sectionGlb); }
            return f;
        }
    }
}
