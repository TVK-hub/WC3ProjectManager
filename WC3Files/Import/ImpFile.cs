using WC3Files.MPQ;
using WC3Files.Triggers;
namespace WC3Files
{
    public class ImpFile
    {
        /// <summary>
        /// Версия формата
        /// </summary>
        public int Version
        {
            set; get;
        }

        /// <summary>
        /// Текстовые триггеры
        /// </summary>
        public List<ImpEntry> Entries
        {
            set; get;
        } = new();

        //Конструктор
        public ImpFile()
        {

        }

        //Загрузить
        public static ImpFile Load(byte[] data)
        {
            ImpReader r = new ImpReader(data);
            return r.ReadFile();
        }
        public static ImpFile LoadFromMPQ(MpqArchive mpq, string path = "war3map.imp")
        {
            byte[] data;
            using (MpqFile f = mpq.OpenFile(path))
            {
                data = f.Read();
            }
            return Load(data);
        }

        //Сохранить
        public void SaveToMPQ(MpqArchive mpq, string path = "war3map.imp")
        {
            //Данные
            byte[] data = null;
            using (MemoryStream stream = new MemoryStream())
            {
                ImpWriter r = new ImpWriter(stream);
                r.WriteFile(this);
                data = stream.ToArray();
            }

            //Добавить в архив
            mpq.CreateFile(path, data);
        }
    }
}
