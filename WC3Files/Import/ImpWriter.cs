using System.Reflection.PortableExecutable;
using System.Text;

namespace WC3Files.Triggers
{
    public class ImpWriter
    {
        //Райтер
        WC3Writer Writer;

        //Конструктор
        public ImpWriter(MemoryStream stream)
        {
            Writer = new WC3Writer(stream);
        }

        //Чтение
        public void WriteFile(ImpFile Imp)
        {
            //Версия
            Writer.Write(Imp.Version);

            //Записи
            Writer.Write(Imp.Entries.Count);
            for (int i = 0; i < Imp.Entries.Count; i++)
            {
                ImpEntry en = Imp.Entries[i];
                Writer.Write((byte)en.Flags);
                Writer.WriteCString(en.Path);
            }
        }
    }
}
