using System.ComponentModel.DataAnnotations;
using WC3Files.Common.MPQ;
using WC3Files.Info;
using WC3ProjectManager.Map.Config;
using WC3ProjectManager.Map.Import;
using WC3ProjectManager.Map.Objects;
using WC3ProjectManager.Map.Strings;
using WC3ProjectManager.Map.Triggers;
using WC3ProjectManager.Project;
namespace WC3ProjectManager.Map
{
    public class WC3Map
    {
        //Файл
        FileInfo File;

        //Версия игры
        public string GameVers
        {
            get;
            private set;
        }

        //Путь к карте
        public string Path
        {
            get { return File.FullName; }
        }

        //Данные
        public W3iFile Info
        {
            get;
            private set;
        }
        public WC3Strings Strings
        {
            private set;
            get;
        }
        public WC3Config Config
        {
            private set;
            get;
        }
        public WC3Triggers Triggers
        {
            get;
            private set;
        }
        public WC3Import Import
        {
            get;
            private set;
        }
        public WC3ObjectsSet Objects
        {
            private set;
            get;
        }

        //Конструктор
        public WC3Map()
        {

        }

        //Загруить
        public static WC3Map FromFile(string path, string gameVer, WC3DataType allowedData = WC3DataType.All)
        {
            return FromFile(new FileInfo(path), gameVer, allowedData);
        }
        public static WC3Map FromFile(FileInfo fInfo, string gameVer, WC3DataType allowedData = WC3DataType.All)
        {
            //Карта
            WC3Map m = new WC3Map();

            //Файл
            m.File = fInfo;

            //Версия
            m.GameVers = gameVer;

            //Архив
            using (MpqArchive mpq = MpqArchive.Load(m.Path))
            {
                m.Info     = W3iFile.LoadFromMPQ(mpq);
                m.Strings  = WC3Strings.LoadFromMPQ(mpq, m.GameVers);
                m.Config   = allowedData.HasFlag(WC3DataType.Config)   ? WC3Config.LoadFromMPQ(mpq, m.Strings)      : new();
                m.Objects  = allowedData.HasFlag(WC3DataType.Objects ) ? WC3ObjectsSet.LoadFromMPQ(mpq, m.GameVers) : new(m.GameVers);
                m.Triggers = allowedData.HasFlag(WC3DataType.Triggers) ? WC3Triggers.LoadFromMPQ(mpq, gameVer)      : new();
                m.Import   = allowedData.HasFlag(WC3DataType.Import  ) ? WC3Import.LoadFromMPQ(mpq)                 : new();
            }
            return m;
        }
        public static WC3Map FirstFromDir(string dir, string gameVer, WC3DataType allowedData = WC3DataType.All)
        {
            //Файлы
            SearchOption opt = SearchOption.TopDirectoryOnly;
            FileInfo[] files = new DirectoryInfo(dir).GetFiles("*.w3x", opt);

            //Карта
            WC3Map map = FromFile(files[0], gameVer, allowedData);
            return map;
        }
        public static WC3Map[] FromDir(string dir, string gameVer, WC3DataType allowedData=WC3DataType.All, bool subfolders=true)
        {
            //Файлы
            SearchOption opt = (subfolders) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            FileInfo[] files = new DirectoryInfo(dir).GetFiles("*.w3x", opt);

            //Карты
            WC3Map[] maps = new WC3Map[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                maps[i] = FromFile(files[i], gameVer, allowedData);
            }
            return maps;
        }

        //Сохранить
        public void Save(WC3DataType dataType=WC3DataType.All)
        {
            using (MpqArchive mpq = MpqArchive.Load(Path))
            {
                //Размер
                int filesCount = 30;
                if (dataType.HasFlag(WC3DataType.Import  )) filesCount += Import.Records.Count;
                mpq.SetMaxFileCount((uint)filesCount);

                //Запись
                if (dataType.HasFlag(WC3DataType.Config)) Config.SaveToMPQ(mpq);
                if (dataType.HasFlag(WC3DataType.Objects)) Objects.SaveToMPQ(mpq);
                if (dataType.HasFlag(WC3DataType.Triggers)) Triggers.SaveToMPQ(mpq, GameVers);
                if (dataType.HasFlag(WC3DataType.Import  )) Import.SaveToMPQ(mpq);
                //mpq.Compact();
            }
        }

        //Копировать
        public WC3Map CopyTo(string path)
        {
            FileInfo f = File.CopyTo(path, true);
            return FromFile(f, GameVers);
        }

        //Удалить
        public void Delete()
        {
            File.Delete();
        }
    }
}
