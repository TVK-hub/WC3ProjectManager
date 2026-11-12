using WC3Files.Common.MPQ;
using WC3Files.Triggers;

namespace WC3Files.Strings
{
    public class WtsFile
    {
        //Словарь
        public Dictionary<string, string> Dict
        {
            get;
            private set;
        } = new();

        //Конструктор
        public WtsFile()
        {

        }

        //Загрузить
        public static WtsFile LoadFromBytes(byte[] data)
        {
            WtsReader r = new WtsReader(data);
            return r.ReadFile();
        }
        public static WtsFile LoadFromMPQ(MpqArchive mpq, string path = "war3map.wts")
        {
            byte[] data;
            using (MpqFile f = mpq.OpenFile(path))
            {
                data = f.Read();
            }
            return LoadFromBytes(data);
        }

        //Сохранить
        //public void SaveToMPQ(MpqArchive mpq, string path = "war3map.wts")
        //{
        //    //Данные
        //    byte[] data = null;
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        WtsWriter r = new WtsWriter(stream);
        //        r.WriteFile(this);
        //        data = stream.ToArray();
        //    }

        //    //Добавить в архив
        //    mpq.CreateFile(path, data);
        //}
    }
}
