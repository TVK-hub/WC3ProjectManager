namespace WC3Files.Common.Ini
{
    public class IniReader
    {
        //Данные
        private IEnumerable<string> Lines;

        //Конструктор
        public IniReader(IEnumerable<string> lines)
        {
            Lines = lines;
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
                    section.Entries.Add(en);
                    int sep = line.IndexOf('=');
                    if (sep > -1)
                    {
                        en.Name = line[..sep].Trim();
                        en.Value = line[(sep + 1)..].Trim();
                    }
                    else
                    {
                        en.Name = line.Trim();
                        en.Value = "";
                    }
                }
            }
            if (sectionGlb.Entries.Count > 0) { f.Sections.Add(sectionGlb); }
            return f;
        }
    }
}
