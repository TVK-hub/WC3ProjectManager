using System.Text;

namespace WC3Files.Triggers
{
    public class WctWriter
    {
        //Райтер
        WC3Writer Writer;

        //Конструктор
        public WctWriter(MemoryStream stream)
        {
            Writer = new WC3Writer(stream);
        }

        //Парсинг
        public void WriteFile(WctFile wct)
        {
            //Версия
            Writer.Write(wct.Version);

            //Данные карты
            if (wct.Version == 1)
            {
                 Writer.WriteCString(wct.Comment);
                 WriteCode(wct.Code);
            }

            //Текстовые триггеры
            Writer.Write(wct.CustomTexts.Count);
            for (int i = 0; i < wct.CustomTexts.Count; i++)
            {
                WriteCode(wct.CustomTexts[i]);
            }
        }
        public void WriteCode(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                int size = Encoding.UTF8.GetByteCount(code)+1;
                Writer.Write(size);         //Размер
                Writer.WriteFxString(code); //Код
                Writer.Write((byte)0);      //Нулевой байт
            }
            else
            {
                Writer.Write(0);            //Размер
            }
        }
    }
}
