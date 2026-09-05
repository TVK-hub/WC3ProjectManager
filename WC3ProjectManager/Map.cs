using WC3Files.MPQ;
using WC3Files.Triggers;
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

        //Конструктор
        public Map()
        {

        }

        //Загруить
        public static Map Load(string path)
        {
            return Load(new FileInfo(path));
        }
        public static Map Load(FileInfo fInfo)
        {
            //Карта
            Map m = new Map();

            //Файл
            m.FileInfo = fInfo;

            //Архив
            using (MpqArchive mpq = MpqArchive.Open(m.Path))
            {
                m.Triggers.LoadFromMPQ(mpq);
            }
            return m;
        }
        public static Map LoadFirstFromDir(string dir)
        {
            //Файлы
            SearchOption opt = SearchOption.TopDirectoryOnly;
            FileInfo[] files = new DirectoryInfo(dir).GetFiles("*.w3x", opt);

            //Карта
            Map map = Load(files[0]);
            return map;
        }
        public static Map[] LoadFromDir(string dir, bool subfolders=true)
        {
            //Файлы
            SearchOption opt = (subfolders) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            FileInfo[] files = new DirectoryInfo(dir).GetFiles("*.w3x", opt);

            //Карты
            Map[] maps = new Map[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                maps[i] = Load(files[i]);
            }
            return maps;
        }

        //Сохранить
        public void Save()
        {
            using (MpqArchive mpq = MpqArchive.Open(Path))
            {
                Triggers.SaveToMPQ(mpq);
                mpq.Compact();
            }
        }
    }
}
