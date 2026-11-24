using System.Text.Json;
using WC3ProjectManager.Map;
using WC3ProjectManager.Map.Config;
using WC3ProjectManager.Map.Import;
using WC3ProjectManager.Map.Jass;
using WC3ProjectManager.Map.Objects;
using WC3ProjectManager.Map.Triggers;
namespace WC3ProjectManager.Project
{
    public class WC3Project
    {
        /// <summary>
        /// Имя проекта
        /// </summary>
        public string Name
        {
            set;
            get;
        }

        /// <summary>
        /// Версия проекта
        /// </summary>
        public string Version
        {
            set;
            get;
        }

        /// <summary>
        /// Директория проекта
        /// </summary>
        public string Dir
        {
            set;
            get;
        }

        /// <summary>
        /// Сборки
        /// </summary>
        public List<WC3ProjectBuild> Builds
        {
            get;
            set;
        } = new();

        /// <summary>
        /// Основная сборка
        /// </summary>
        public WC3ProjectBuild BaseBuild
        {
            get
            {
                return Builds[BaseBuildIndex];
            }
        }
        public int BaseBuildIndex = 0;

        /// <summary>
        /// Пути к разделам проекта
        /// </summary>
        public WC3ProjectPaths Paths
        {
            get;
            set;
        } = new();

        //Конструктор
        public WC3Project()
        {

        }
        public static WC3Project? Current;

        //Загрузка
        public static WC3Project Load(string dir)
        {
            //Загрузка из JSON
            string json = File.ReadAllText(dir+"/project.json");
            WC3Project? prj = JsonSerializer.Deserialize<WC3Project>(json);

            //Директория
            prj.Dir = dir;
            prj.Paths.Init(dir);
            return prj;
        }
        public static bool TryLoad(string path, out WC3Project? prj)
        {
            bool res = true;
            try
            {
                prj = Load(path);
            }
            catch (Exception exc)
            {
                prj = null;
                res = false;
#if DEBUG
                throw exc;
#endif
            }
            return res;
        }

        /// <summary>
        /// Извлечь данные из карты.
        /// </summary>
        public void Extract(string path, WC3DataType dataType)
        {
            //Карта
            WC3Map map;
            if (string.IsNullOrEmpty(path))
            {
                map = WC3Map.FirstFromDir(Paths.Maps, BaseBuild.GameVersion, dataType);
            }
            else
            {
                if (!File.Exists(path))
                {
                    path = Paths.Maps + "\\" + path;
                }
                map = WC3Map.FromFile(path, BaseBuild.GameVersion, dataType);
            }

            //Конфигурация
            if (dataType.HasFlag(WC3DataType.Config))
            {
                map.Config.SaveToProject(Paths.Config);
            }

            //Объекты
            if (dataType.HasFlag(WC3DataType.Objects))
            {
                map.Objects.SaveToProject(Paths.Objects.First());
            }

            //Триггеры
            if (dataType.HasFlag(WC3DataType.Triggers))
            {
                map.Triggers.SaveToProject(Paths.Triggers.First());
            }

            //Импорт
            if (dataType.HasFlag(WC3DataType.Import))
            {
                map.Import.SaveToProject(Paths.Import);
            }
        }

        /// <summary>
        /// Внедрить триггеры проекта в карты.
        /// </summary>
        public void Inject(string path, WC3DataType dataType)
        {
            //Данные на внедрение
            WC3Config?     cf = dataType.HasFlag(WC3DataType.Config) ? WC3Config.LoadFromDir(Paths.Config) : null;
            WC3ObjectsSet? objs = dataType.HasFlag(WC3DataType.Objects) ? WC3ObjectsSet.LoadFromDirs(Paths.Objects, BaseBuild.GameVersion) : null;
            WC3Triggers?   triggers = dataType.HasFlag(WC3DataType.Triggers) ? WC3Triggers.LoadFromDirs(Paths.Triggers) : null;
            WC3Import?     import   = dataType.HasFlag(WC3DataType.Import)   ? WC3Import.LoadFromDir(Paths.Import) : null;

            //Карты
            WC3Map[] maps;
            if (string.IsNullOrEmpty(path))
            {
                maps = WC3Map.FromDir(Paths.Maps, BaseBuild.GameVersion, dataType);
            }
            else
            {
                if (!File.Exists(path))
                {
                    path = Paths.Maps+"\\"+path;
                }
                maps = [WC3Map.FromFile(path, BaseBuild.GameVersion, dataType)];
            }

            //Перечисление
            foreach (WC3Map m in maps)
            {
                //Конфигурация
                if (cf != null) { m.Config.Inject(cf); }

                //Объекты
                if (objs!=null) { m.Objects.Inject(objs); }

                //Триггеры
                if (triggers != null) { m.Triggers.Inject(triggers); }

                //Импорт
                if (import!=null) { m.Import.Inject(import); }

                //Сохранить карту
                m.Save(dataType);
            }
        }

        /// <summary>
        /// Очистить триггеры в картах.
        /// </summary>
        public void Clear(string path, WC3DataType dataType)
        {
            //Карты
            WC3Map[] maps;
            if (string.IsNullOrEmpty(path))
            {
                maps = WC3Map.FromDir(Paths.Maps, BaseBuild.GameVersion, dataType);
            }
            else
            {
                if (!File.Exists(path))
                {
                    path = Paths.Maps + "\\" + path;
                }
                maps = [WC3Map.FromFile(path, BaseBuild.GameVersion, dataType)];
            }

            //Перечисление
            foreach (WC3Map m in maps)
            {
                //Конфигурация
                if (dataType.HasFlag(WC3DataType.Config)) { m.Config.Reset(); }

                //Объекты
                if (dataType.HasFlag(WC3DataType.Objects)){ m.Objects.Reset(); }

                //Триггеры
                if (dataType.HasFlag(WC3DataType.Triggers)){ m.Triggers.Clear(); }

                //Импорт
                if (dataType.HasFlag(WC3DataType.Import)){ m.Import.Clear(); }

                //Сохранить карту
                m.Save(dataType);
            }
        }

        /// <summary>
        /// Проверить триггеры проекта.
        /// </summary>
        public void Check()
        {
            //Тестовый файл
            WC3Triggers triggers = WC3Triggers.LoadFromDirs(Paths.Triggers);
            JassFile testFile = JassFile.Build(Dir + "\\test.j", triggers);

            //Проверка
            testFile.Check();

            //Удаление
            testFile.Delete();
        }
    }
}