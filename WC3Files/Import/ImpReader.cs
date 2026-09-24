namespace WC3Files
{
    public class ImpReader
    {
        //Ридер
        WC3Reader Reader;

        //Конструктор
        public ImpReader(byte[] data)
        {
            Reader = new WC3Reader(data);
        }

        //Парсинг
        public ImpFile ReadFile()
        {
            //Класс
            var imp = new ImpFile();

            //Версия
            imp.Version = Reader.ReadInt32();

            //Записи
            int count = Reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                ImpEntry en = new ImpEntry();
                en.Flags = (ImpFlags)Reader.ReadByte();
                en.Path = Reader.ReadCString();
                imp.Entries.Add(en);
            }

            //Вернуть
            return imp;
        }
    }
}
