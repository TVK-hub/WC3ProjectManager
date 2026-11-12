using WC3Files.Common.MPQ;
using WC3Files.Triggers;
namespace WC3ProjectManager.Map.Triggers
{
    public class WC3Triggers
    {
        //Версия
        public int GameVersion
        {
            set;
            get;
        }
        public int WtgVersion
        {
            set;
            get;
        }
        public int WctVersion
        {
            set;
            get;
        }

        //Заголовок
        public WC3Trigger Header
        {
            set;
            get;
        } = new();

        //Переменные
        public List<WtgVariable> GlobalVars
        {
            set;
            get;
        }

        //Категории
        public List<WC3TriggerCategory> Categories
        {
            get;
        } = new();

        //Триггеры
        public IEnumerable<IWC3Trigger> All => Categories.SelectMany(c => c.Triggers);

        //Внедрить
        public void Inject(WC3Triggers src)
        {
            //Заголовок
            if (src.Header.Code!="")       { Header.Code = src.Header.Code; }
            if (src.Header.Description!=""){ Header.Description = src.Header.Description; }

            //Категории
            foreach (WC3TriggerCategory srcCat in src.Categories)
            {
                //Категория
                WC3TriggerCategory? cat = Categories.FirstOrDefault(kv => kv.Name == srcCat.Name);
                if (cat != null)
                {
                    //Очистить триггеры
                    cat.Triggers.Clear();
                }
                else
                {
                    //Создать категорию
                    cat = new WC3TriggerCategory();
                    cat.Id = 1;
                    cat.Name = srcCat.Name;

                    //Рассчитать ID
                    foreach (WC3TriggerCategory c in Categories)
                    {
                        if (cat.Id <= c.Id) { cat.Id = c.Id + 1; }
                    }

                    //Добавить в список
                    Categories.Insert(0, cat);
                }

                //Добавить триггеры
                cat.Triggers.AddRange(srcCat.Triggers);
            }
        }

        //Очистить
        public void Clear()
        {
            GlobalVars.Clear();
            Header.Code = "";
            Header.Description = "";
            Categories.Clear();
        }

        //Загрузить
        public static WC3Triggers Load(WtgFile wtg, WctFile wct)
        {
            //Объект
            WC3Triggers trgs = new WC3Triggers();

            //Версия
            trgs.WtgVersion = wtg.Version;
            trgs.WctVersion = wct.Version;
            trgs.GameVersion = wtg.GameVersion;

            //Заголовок
            trgs.Header.Code = wct.Code;
            trgs.Header.Description = wct.Comment;

            //Переменные
            trgs.GlobalVars = wtg.GlobalVars;

            //Категории
            for (int i = 0; i < wtg.Categories.Count; i++)
            {
                WtgCategory wtgCategory = wtg.Categories[i];
                WC3TriggerCategory c = new WC3TriggerCategory();
                c.Id = wtgCategory.Index;
                c.Name = wtgCategory.Name;
                trgs.Categories.Add(c);
            }

            //Триггеры
            for (int i = 0; i < wtg.Triggers.Count; i++)
            {
                IWC3Trigger itrg;
                WtgTrigger wtgTrg = wtg.Triggers[i];
                if (!wtgTrg.IsComment)
                {
                    //Триггер
                    WC3Trigger trg = new WC3Trigger();
                    itrg = trg;

                    //Флаги
                    trg.Enabled = wtgTrg.IsEnabled;
                    trg.RunOnInit = wtgTrg.RunOnInit;
                    trg.IsInitiallyOff = wtgTrg.IsInitiallyOff;

                    //Код
                    if (wtgTrg.IsCustomText)
                    {
                        trg.Code = wct.CustomTexts[i];
                    }
                    //GUI
                    else
                    {
                        trg.GUI = wtgTrg.ECA;
                    }
                }
                else
                {
                    //Комментарий
                    itrg = new WC3TriggerComment();
                }
                itrg.Name = wtgTrg.Name;
                itrg.Description = wtgTrg.Description;

                //В категорию
                trgs.Categories.First(itm=>itm.Id==wtgTrg.CategoryId).Triggers.Add(itrg);
            }
            return trgs;
        }
        public static WC3Triggers LoadFromMPQ(MpqArchive mpq, string gameVer)
        {
            WtgFile wtg = WtgFile.LoadFromMPQ(mpq, gameVer);
            WctFile wct = WctFile.LoadFromMPQ(mpq);
            return Load(wtg, wct);
        }
        public static WC3Triggers LoadFromDir(string dir)
        {
            //Триггеры
            WC3Triggers t = new WC3Triggers();

            //Заголовок
            string? codePath = Directory.GetFiles(dir, "code.*").FirstOrDefault();
            if (!string.IsNullOrEmpty(codePath))
            {
                t.Header = WC3Trigger.FromFile(codePath);
            }

            //Папки
            DirectoryInfo[] folders = new DirectoryInfo(dir).GetDirectories();
            for (int i = 0; i < folders.Length; i++)
            {
                //Папка
                DirectoryInfo folder = folders[i];
                
                //Файлы
                List<WC3Trigger> trgsLst = new(WC3Trigger.FromDir(folder.FullName));

                //Категория
                WC3TriggerCategory cat = new WC3TriggerCategory();
                cat.Id = 1 + i;
                cat.Name = folder.Name;
                cat.Triggers.AddRange(trgsLst);
                t.Categories.Add(cat);
            }
            return t;
        }

        //Сохранить
        public void SaveToMPQ(MpqArchive mpq, string gameVer)
        {
            //Файлы
            WtgFile wtg = new();
            WctFile wct = new();

            //Заголовок
            wct.Code = Header.Code;
            wct.Comment = Header.Description;

            //Версия
            wtg.Version = WtgVersion;
            wtg.GameVersion = GameVersion;
            wct.Version = WctVersion;

            //Переменные
            wtg.GlobalVars = GlobalVars;

            //Категории
            foreach (WC3TriggerCategory cat in Categories)
            {
                //Категория WTG
                WtgCategory wtgC = new();
                wtgC.Index = cat.Id;
                wtgC.Name = cat.Name;
                wtg.Categories.Add(wtgC);

                //Триггеры
                foreach (IWC3Trigger itrg in cat.Triggers)
                {
                    //Триггер WTG
                    WtgTrigger wtgTrg = new WtgTrigger();
                    wtgTrg.Name = itrg.Name;
                    wtgTrg.Description = itrg.Description;
                    wtgTrg.CategoryId = cat.Id;
                    if (itrg is WC3Trigger trg)
                    {
                        wtgTrg.IsEnabled = trg.Enabled;
                        wtgTrg.RunOnInit = trg.RunOnInit;
                        wtgTrg.IsInitiallyOff = trg.IsInitiallyOff;
                        wtgTrg.IsCustomText = trg.GUI == null;
                        if (!wtgTrg.IsCustomText)
                        {
                            wtgTrg.ECA = trg.GUI;
                        }
                    }
                    else if (itrg is WC3TriggerComment com)
                    {
                        wtgTrg.IsComment = true;
                    }
                    wtg.Triggers.Add(wtgTrg);

                    //Код WCT
                    string code = "";
                    if (itrg is WC3Trigger)
                    {
                        code = ((WC3Trigger)itrg).Code;
                    }
                    wct.CustomTexts.Add(code);
                }
            }

            //Сохранение в архив
            wtg.SaveToMPQ(mpq, gameVer);
            wct.SaveToMPQ(mpq);
        }
    }
}