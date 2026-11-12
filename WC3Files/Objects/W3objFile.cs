using WC3Files.Common.MPQ;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace WC3Files.Objects
{
    public class W3objFile
    {
        //Версия структуры
        public int Version;

        //Изменения стандартных объектов
        public List<W3objMod> StandartMods
        {
            get;
            private set;
        }

        //Изменения нестандартных объектов
        public List<W3objMod> CustomMods
        {
            get;
            private set;
        }

        //Конструктор
        public W3objFile()
        {
            StandartMods = new List<W3objMod>();
            CustomMods = new List<W3objMod>();
        }

        //Загрузить
        public static W3objFile Load(byte[] data, W3objTypes objType)
        {
            W3objReader r = new W3objReader(data, objType);
            return r.ReadFile();
        }
        public static W3objFile? LoadFromMPQ(MpqArchive mpq, string path, W3objTypes objType)
        {
            W3objFile? w3obj = null;
            if (mpq.TryOpenFile(path, out MpqFile? f))
            {
                byte[] data = f.Read();
                f.Close();
                w3obj = Load(data, objType);
            }
            return w3obj;
        }

        //Сохранить
        public void SaveToMPQ(MpqArchive mpq, string path, W3objTypes objType)
        {
            //Данные
            byte[] data = null;
            using (MemoryStream stream = new MemoryStream())
            {
                W3objWriter r = new W3objWriter(stream, objType);
                r.WriteFile(this);
                data = stream.ToArray();
            }

            //Добавить в архив
            mpq.CreateFile(path, data);
        }
    }
}
