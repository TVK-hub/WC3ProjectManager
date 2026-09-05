namespace WC3Files.Triggers
{
    public class WctReader
    {
        //Ридер
        WC3Reader Reader;

        //Конструктор
        public WctReader(byte[] data)
        {
            Reader = new WC3Reader(data);
        }

        //Парсинг
        public WctFile ReadFile()
        {
            //Класс
            var wct = new WctFile();

            //Версия
            wct.Version = Reader.ReadInt32();

            //Данные карты
            if (wct.Version == 1)
            {
                wct.Comment = Reader.ReadCString();
                wct.Code = ReadCode();
            }

            //Текстовые триггеры
            int txtCount = Reader.ReadInt32();
            for (int i = 0; i < txtCount; i++)
            {
                string code = ReadCode();
                wct.CustomTexts.Add(code);
            }

            //Вернуть
            return wct;
        }
        public string ReadCode()
        {
            string code = "";
            int size = Reader.ReadInt32();
            if (size > 0)
            {
                code = Reader.ReadFxString(size-1); //Код
                Reader.ReadByte();                  //Нулевой байт
            }
            return code;
        }
    }
}
