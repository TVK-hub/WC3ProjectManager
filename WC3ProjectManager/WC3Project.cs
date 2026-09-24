using System.Text.Json;
using WC3Files.MPQ;
using WC3ProjectManager.Jass;

namespace WC3ProjectManager
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

        //Директории
        private WC3ProjectDirectories Dirs;

        //Конструктор
        public WC3Project()
        {

        }
        public static WC3Project Load(string path)
        {
            //Загрузка из JSON
            string json = File.ReadAllText(path+"/project.json");
            WC3Project prj = JsonSerializer.Deserialize<WC3Project>(json);

            //Путь
            prj.Path = path;

            //Директории
            prj.Dirs = new WC3ProjectDirectories(path);
            return prj;
        }
        public static WC3Project Current;

        /// <summary>
        /// Извлечь данные из карты.
        /// </summary>
        public void Extract(string path, WC3DataType dataType)
        {
            //Карта
            Map map;
            if (string.IsNullOrEmpty(path))
            {
                map = Map.FirstFromDir(Dirs.Maps);
            }
            else
            {
                map = Map.FromFile(path);
            }

            //Триггеры
            if (dataType.HasFlag(WC3DataType.Triggers))
            {
                map.Triggers.Header.Save($"{Dirs.Triggers}\\code.j");
                foreach (TriggerCategory cat in map.Triggers.Categories)
                {
                    foreach (ITrigger itrg in cat.Triggers)
                    {
                        if (itrg is Trigger trg)
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
                //TODO
            }
        }

        /// <summary>
        /// Внедрить триггеры проекта в карты.
        /// </summary>
        public void Inject(string path, WC3DataType dataType)
        {
            //Данные на внедрение
            Triggers? triggers = dataType.HasFlag(WC3DataType.Triggers) ? Triggers.LoadFromDir(Name, Dirs.Triggers) : null;
            Import?   import   = dataType.HasFlag(WC3DataType.Import)   ? Import.LoadFromDir(Dirs.Import)           : null;

            //Карты
            Map[] maps;
            if (string.IsNullOrEmpty(path))
            {
                maps = Map.FromDir(Dirs.Maps);
            }
            else
            {
                maps = [Map.FromFile(path)];
            }

            //Перечисление
            foreach (Map m in maps)
            {
                //Триггеры
                if (triggers != null) { m.Triggers.Inject(triggers); }

                //Импорт
                if (import!=null) { m.Import.Inject(import); }

                //Объекты
                if (dataType.HasFlag(WC3DataType.Objects)) { /*TODO*/ }

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
            Map[] maps;
            if (string.IsNullOrEmpty(path))
            {
                maps = Map.FromDir(Dirs.Maps);
            }
            else
            {
                maps = [Map.FromFile(path)];
            }

            //Перечисление
            foreach (Map m in maps)
            {
                //Триггеры
                if (dataType.HasFlag(WC3DataType.Triggers)){ m.Triggers.Clear(); }

                //Импорт
                if (dataType.HasFlag(WC3DataType.Import)){ m.Import.Clear(); }

                //Объекты
                if (dataType.HasFlag(WC3DataType.Objects)){ /*TODO*/ }

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
            Triggers triggers = Triggers.LoadFromDir(Name, Dirs.Triggers);
            JassFile testFile = JassFile.Build(Path + "\\test.j", triggers);

            //Проверка
            testFile.Check();

            //Удаление
            testFile.Delete();
        }
    }
}