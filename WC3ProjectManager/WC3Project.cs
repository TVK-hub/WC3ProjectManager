using System.Reflection.PortableExecutable;
using System.Text.Json;

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
                map = Map.LoadFirstFromDir(Dirs.Maps);
            }
            else
            {
                map = Map.Load(path);
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
            List<Trigger> triggers = new(Trigger.FromDir(Dirs.Triggers));
            Trigger header = null; int hInx = triggers.FindIndex(trg=>trg.Name=="code");
            if (hInx > -1) { header = triggers[hInx]; triggers.RemoveAt(hInx); }

            //Карты
            Map[] maps = Map.LoadFromDir(Dirs.Maps);
            foreach (Map m in maps)
            {
                //Заголовок
                if (header != null)
                {
                    m.Triggers.Header = header;
                }

                //Категория
                TriggerCategory cat = m.Triggers.Categories.FirstOrDefault(kv => kv.Name == Name);
                if (cat != null)
                {
                    cat.Triggers.Clear();
                }
                else
                {
                    cat = new TriggerCategory();
                    cat.Id = 1;
                    foreach (TriggerCategory c in m.Triggers.Categories)
                    {
                        if (cat.Id <= c.Id) { cat.Id = c.Id + 1; }
                    }
                    cat.Name = Name;
                    m.Triggers.Categories.Insert(0, cat);
                }

                //Добавить триггеры
                cat.Triggers.AddRange(triggers);

                //Сохранить карту
                m.Save();
            }
        }

        /// <summary>
        /// Очистить триггеры во всех картах.
        /// </summary>
        public void Clear()
        {
            Map[] maps = Map.LoadFromDir(Dirs.Maps);
            foreach (Map m in maps)
            {
                //Очистка
                m.Triggers.GlobalVars.Clear();
                m.Triggers.Header.Code = "";
                m.Triggers.Header.Description = "";
                m.Triggers.Categories.Clear();

                //Сохранить карту
                m.Save();
            }
        }
    }
}