using System.Text;

namespace WC3Files.Common.Ini
{
    public class IniWriter : IDisposable
    {
        //Райтер
        StreamWriter Writer;

        //Конструктор
        public IniWriter(Stream stream)
        {
            Writer = new StreamWriter(stream, new UTF8Encoding(false));
        }
        public void Dispose()
        {
            Writer.Dispose();
        }

        //Запись
        public void WriteFile(IniFile f)
        {
            for (int secInx = 0; secInx < f.Sections.Count; secInx++)
            {
                //Секция
                IniSection sec = f.Sections[secInx];
                if (!string.IsNullOrEmpty(sec.Name))
                {
                    Writer.Write($"[{sec.Name}]\n");
                }

                //Записи
                string del = (!f.SpacesAroundDelimiter) ? "=" : " = ";
                for (int eInx = 0; eInx < sec.Entries.Count; eInx++)
                {
                    IniEntry e = sec.Entries[eInx];
                    Writer.Write($"{e.Name}{del}{e.Value}");
                    if (eInx < sec.Entries.Count - 1) { Writer.Write("\n"); }
                }
                if (secInx < f.Sections.Count - 1) { Writer.Write("\n\n"); }
            }
        }
    }
}
