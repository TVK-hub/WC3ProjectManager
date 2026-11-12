using System.Text;

namespace WC3Files.Common.Ini
{
    public class IniWriter : IDisposable
    {
        //Райтер
        FileStream Stream;
        StreamWriter Writer;

        //Конструктор
        public IniWriter(string path)
        {
            Stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            Writer = new StreamWriter(Stream, Encoding.UTF8);
        }
        public void Dispose()
        {
            Writer.Dispose();
            Stream.Dispose();
        }

        //Чтение
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
                for (int eInx = 0; eInx < sec.Entries.Count; eInx++)
                {
                    IniEntry e = sec.Entries[eInx];
                    Writer.Write($"{e.Name} = {e.Value}");
                    if (eInx < sec.Entries.Count - 1) { Writer.Write("\n"); }
                }
                if (secInx < f.Sections.Count - 1) { Writer.Write("\n\n"); }
            }
        }
    }
}
