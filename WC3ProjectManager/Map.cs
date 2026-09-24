using WC3Files.MPQ;
namespace WC3ProjectManager
{
    public class Map
    {
        //Файл
        FileInfo FileInfo;

        //Путь к карте
        public string Path
        {
            get { return FileInfo.FullName; }
        }

        //Данные
        public Triggers Triggers
        {
            get;
            set;
        } = new();
        public Import Import
        {
            get;
            set;
        } = new();

        //Конструктор
        public Map()
        {

        }

        //Загруить
        public static Map FromFile(string path)
        {
            return FromFile(new FileInfo(path));
        }
        public static Map FromFile(FileInfo fInfo)
        {
            //Карта
            Map m = new Map();

            //Файл
            m.FileInfo = fInfo;

            //Архив
            using (MpqArchive mpq = MpqArchive.Load(m.Path))
            {
                m.Triggers.LoadFromMPQ(mpq);
                m.Import.LoadFromMPQ(mpq);
            }
            return m;
        }
        public static Map FirstFromDir(string dir)
        {
            //Файлы
            SearchOption opt = SearchOption.TopDirectoryOnly;
            FileInfo[] files = new DirectoryInfo(dir).GetFiles("*.w3x", opt);

            //Карта
            Map map = FromFile(files[0]);
            return map;
        }
        public static Map[] FromDir(string dir, bool subfolders=true)
        {
            //Файлы
            SearchOption opt = (subfolders) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            FileInfo[] files = new DirectoryInfo(dir).GetFiles("*.w3x", opt);

            //Карты
            Map[] maps = new Map[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                maps[i] = FromFile(files[i]);
            }
            return maps;
        }

        //Сохранить
        public void Save(WC3DataType dataType=WC3DataType.All)
        {
            using (MpqArchive mpq = MpqArchive.Load(Path))
            {
                if (dataType.HasFlag(WC3DataType.Triggers)) Triggers.SaveToMPQ(mpq);
                if (dataType.HasFlag(WC3DataType.Import  )) Import.SaveToMPQ(mpq);
                //mpq.Compact();
            }
        }

        //Копировать
        public Map CopyTo(string path)
        {
            FileInfo f = FileInfo.CopyTo(path, true);
            return FromFile(f);
        }

        //Удалить
        public void Delete()
        {
            FileInfo.Delete();
        }
    }
}
