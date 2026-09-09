using System.Reflection.PortableExecutable;
using System.Text.Json;
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
        /// Извлечь триггеры из карты.
        /// </summary>
        public void Extract(string path="")
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

        /// <summary>
        /// Внедрить триггеры проекта в карты.
        /// </summary>
        public void Inject()
        {
            //Триггеры на внедрение
            Triggers triggers = Triggers.FromDir(Name, Dirs.Triggers);

            //Карты
            Map[] maps = Map.FromDir(Dirs.Maps);
            foreach (Map m in maps)
            {
                //Внедрить
                m.Triggers.Inject(triggers);

                //Сохранить карту
                m.Save();
            }
        }

        /// <summary>
        /// Очистить триггеры во всех картах.
        /// </summary>
        public void Clear()
        {
            Map[] maps = Map.FromDir(Dirs.Maps);
            foreach (Map m in maps)
            {
                //Очистка
                m.Triggers.Clear();

                //Сохранить карту
                m.Save();
            }
        }

        /// <summary>
        /// Проверить триггеры проекта.
        /// </summary>
        public void Check()
        {
            //Тестовый файл
            Triggers triggers = Triggers.FromDir(Name, Dirs.Triggers);
            JassFile testFile = JassFile.Build(Path + "\\test.j", triggers);

            //Проверка
            testFile.Check();

            //Удаление
            testFile.Delete();
        }
    }
}