using System.Text.Json;
using WC3ProjectManager.Map;
using WC3ProjectManager.Map.Import;
using WC3ProjectManager.Map.Jass;
using WC3ProjectManager.Map.Objects;
using WC3ProjectManager.Map.Strings;
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
        /// Путь к проекту
        /// </summary>
        public string Path
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
        /// Директории проекта
        /// </summary>
        public WC3ProjectDirectories Dirs
        {
            private set;
            get;
        }

        //Конструктор
        public WC3Project()
        {

        }
        public static WC3Project? Current;

        //Загрузка
        public static WC3Project Load(string path)
        {
            //Загрузка из JSON
            string json = File.ReadAllText(path+"/project.json");
            WC3Project? prj = JsonSerializer.Deserialize<WC3Project>(json);

            //Директория
            prj.Path = path;
            prj.Dirs = new WC3ProjectDirectories(prj);
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
                map = WC3Map.FirstFromDir(Dirs.Maps, BaseBuild.GameVersion, dataType);
            }
            else
            {
                if (!File.Exists(path))
                {
                    path = Dirs.Maps + "\\" + path;
                }
                map = WC3Map.FromFile(path, BaseBuild.GameVersion, dataType);
            }

            //Триггеры
            if (dataType.HasFlag(WC3DataType.Triggers))
            {
                map.Triggers.Header.Save($"{Dirs.Triggers}\\code.j");
                foreach (WC3TriggerCategory cat in map.Triggers.Categories)
                {
                    foreach (IWC3Trigger itrg in cat.Triggers)
                    {
                        if (itrg is WC3Trigger trg)
                        {
                            trg.Save($"{Dirs.Triggers}\\{cat.Name}\\{trg.Name}.j");
                        }
                    }
                }
            }

            //Импорт
            if (dataType.HasFlag(WC3DataType.Import))
            {
                map.Import.SaveToDir(Dirs.Import);
            }

            //Объекты
            if (dataType.HasFlag(WC3DataType.Objects))
            {
                map.Objects.SaveToDir(Dirs.Objects, map.Strings);
            }
        }

        /// <summary>
        /// Внедрить триггеры проекта в карты.
        /// </summary>
        public void Inject(string path, WC3DataType dataType)
        {
            //Данные на внедрение
            WC3Triggers?   triggers = dataType.HasFlag(WC3DataType.Triggers) ? WC3Triggers.LoadFromDir(Dirs.Triggers) : null;
            WC3Import?     import   = dataType.HasFlag(WC3DataType.Import)   ? WC3Import.LoadFromDir(Dirs.Import) : null;
            WC3ObjectsSet? objs     = dataType.HasFlag(WC3DataType.Objects)  ? WC3ObjectsSet.LoadFromDir(Dirs.Objects, BaseBuild.GameVersion) : null;

            //Карты
            WC3Map[] maps;
            if (string.IsNullOrEmpty(path))
            {
                maps = WC3Map.FromDir(Dirs.Maps, BaseBuild.GameVersion, dataType);
            }
            else
            {
                if (!File.Exists(path))
                {
                    path = Dirs.Maps+"\\"+path;
                }
                maps = [WC3Map.FromFile(path, BaseBuild.GameVersion, dataType)];
            }

            //Перечисление
            foreach (WC3Map m in maps)
            {
                //Триггеры
                if (triggers != null) { m.Triggers.Inject(triggers); }

                //Импорт
                if (import!=null) { m.Import.Inject(import); }

                //Объекты
                if (objs!=null) { m.Objects.Inject(objs);  }

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
                maps = WC3Map.FromDir(Dirs.Maps, BaseBuild.GameVersion, dataType);
            }
            else
            {
                if (!File.Exists(path))
                {
                    path = Dirs.Maps + "\\" + path;
                }
                maps = [WC3Map.FromFile(path, BaseBuild.GameVersion, dataType)];
            }

            //Перечисление
            foreach (WC3Map m in maps)
            {
                //Триггеры
                if (dataType.HasFlag(WC3DataType.Triggers)){ m.Triggers.Clear(); }

                //Импорт
                if (dataType.HasFlag(WC3DataType.Import)){ m.Import.Clear(); }

                //Объекты
                if (dataType.HasFlag(WC3DataType.Objects)){ m.Objects.Reset(); }

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
            WC3Triggers triggers = WC3Triggers.LoadFromDir(Dirs.Triggers);
            JassFile testFile = JassFile.Build(Path + "\\test.j", triggers);

            //Проверка
            testFile.Check();

            //Удаление
            testFile.Delete();
        }
    }
}