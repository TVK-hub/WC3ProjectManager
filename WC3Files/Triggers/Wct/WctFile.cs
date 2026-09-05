using WC3Files.MPQ;

namespace WC3Files.Triggers
{
    public class WctFile
    {
        /// <summary>
        /// Версия формата
        /// </summary>
        public int Version
        {
            set; get;
        }

        /// <summary>
        /// Комментарий карты
        /// </summary>
        public string Comment
        {
            set; get;
        }

        /// <summary>
        /// Код карты
        /// </summary>
        public string Code
        {
            set; get;
        }

        /// <summary>
        /// Текстовые триггеры
        /// </summary>
        public List<string> CustomTexts
        {
            set; get;
        } = new();

        //Конструктор
        public WctFile()
        {
            CustomTexts = new();
        }

        //Загрузить
        public static WctFile Load(byte[] data)
        {
            WctReader r = new WctReader(data);
            return r.ReadFile();
        }
        public static WctFile LoadFromMPQ(MpqArchive mpq, string path = "war3map.wct")
        {
            byte[] data;
            using (MpqFile f = mpq.OpenFile(path))
            {
                data = f.Read();
            }
            return Load(data);
        }

        //Сохранить
        public void SaveToMPQ(MpqArchive mpq, string path = "war3map.wct")
        {
            //Данные
            byte[] data = null;
            using (MemoryStream stream = new MemoryStream())
            {
                WctWriter r = new WctWriter(stream);
                r.WriteFile(this);
                data = stream.ToArray();
            }

            //Добавить в архив
            mpq.CreateFile(path, data);
        }
    }
}
