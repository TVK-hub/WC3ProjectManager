using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WC3Files.MPQ;

namespace WC3Files.Triggers
{
    //Конструктор
    public class WtgFile
    {
        //Версия структуры
        public int Version;

        //Категории
        public List<WtgCategory> Categories;

        //Версия игры
        public int GameVersion;

        //Глобальные переменные
        public List<WtgVariable> GlobalVars;

        //Триггеры
        public List<WtgTrigger> Triggers;

        //Конструктор
        public WtgFile()
        {
            Categories = new List<WtgCategory>();
            GlobalVars = new List<WtgVariable>();
            Triggers = new List<WtgTrigger>();
        }

        //Загрузить
        public static WtgFile Load(byte[] data)
        {
            WtgReader r = new WtgReader(data);
            return r.ReadFile();
        }
        public static WtgFile LoadFromMPQ(MpqArchive mpq, string path="war3map.wtg")
        {
            byte[] data;
            using (MpqFile f = mpq.OpenFile(path))
            {
                data = f.Read();
            }
            return Load(data);
        }

        //Сохранить
        public void SaveToMPQ(MpqArchive mpq, string path = "war3map.wtg")
        {
            //Данные
            byte[] data = null;
            using (MemoryStream stream = new MemoryStream())
            {
                WtgWriter r = new WtgWriter(stream);
                r.WriteFile(this);
                data = stream.ToArray();
            }

            //Добавить в архив
            mpq.CreateFile(path, data);
        }
    }
}
