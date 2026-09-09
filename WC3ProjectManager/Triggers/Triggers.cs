using System.Net.Http.Headers;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;
using WC3Files.MPQ;
using WC3Files.Triggers;
namespace WC3ProjectManager
{
    public class Triggers
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
        public Trigger Header
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
        public List<TriggerCategory> Categories
        {
            get;
        } = new();

        //Триггеры
        public IEnumerable<ITrigger> All => Categories.SelectMany(c => c.Triggers);

        //Внедрить
        public void Inject(Triggers src)
        {
            //Заголовок
            if (src.Header.Code!="")       { Header.Code = src.Header.Code; }
            if (src.Header.Description!=""){ Header.Description = src.Header.Description; }

            //Категории
            foreach (TriggerCategory srcCat in src.Categories)
            {
                //Категория
                TriggerCategory? cat = Categories.FirstOrDefault(kv => kv.Name == srcCat.Name);
                if (cat != null)
                {
                    //Очистить триггеры
                    cat.Triggers.Clear();
                }
                else
                {
                    //Создать категорию
                    cat = new TriggerCategory();
                    cat.Id = 1;
                    cat.Name = srcCat.Name;

                    //Рассчитать ID
                    foreach (TriggerCategory c in Categories)
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
        private void Load(WtgFile wtg, WctFile wct)
        {
            //Версия
            WtgVersion = wtg.Version;
            WctVersion = wct.Version;
            GameVersion = wtg.GameVersion;

            //Заголовок
            Header.Code = wct.Code;
            Header.Description = wct.Comment;

            //Переменные
            GlobalVars = wtg.GlobalVars;

            //Категории
            for (int i = 0; i < wtg.Categories.Count; i++)
            {
                WtgCategory wtgCategory = wtg.Categories[i];
                TriggerCategory c = new TriggerCategory();
                c.Id = wtgCategory.Index;
                c.Name = wtgCategory.Name;
                Categories.Add(c);
            }

            //Триггеры
            for (int i = 0; i < wtg.Triggers.Count; i++)
            {
                ITrigger itrg = null;
                WtgTrigger wtgTrg = wtg.Triggers[i];
                if (!wtgTrg.IsComment)
                {
                    //Триггер
                    Trigger trg = new Trigger();
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
                    itrg = new TriggerComment();
                }
                itrg.Name = wtgTrg.Name;
                itrg.Description = wtgTrg.Description;

                //В категорию
                Categories.First(itm=>itm.Id==wtgTrg.CategoryId).Triggers.Add(itrg);
            }
        }
        public void LoadFromMPQ(MpqArchive mpq)
        {
            WtgFile wtg = WtgFile.LoadFromMPQ(mpq);
            WctFile wct = WctFile.LoadFromMPQ(mpq);
            Load(wtg, wct);
        }
        public static Triggers FromDir(string catName, string path)
        {
            //Триггеры
            Triggers t = new Triggers();

            //Чтение файлов
            List<Trigger> trgsLst = new(Trigger.FromDir(path));
            
            //Заголовок
            int hInx = trgsLst.FindIndex(trg => trg.Name == "code");
            if (hInx > -1) { t.Header = trgsLst[hInx]; trgsLst.RemoveAt(hInx); }

            //Триггеры
            TriggerCategory cat = new TriggerCategory();
            cat.Id = 1;
            cat.Name = catName;
            cat.Triggers.AddRange(trgsLst);
            t.Categories.Add(cat);
            return t;
        }

        //Сохранить
        public void SaveToMPQ(MpqArchive mpq)
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
            foreach (TriggerCategory cat in Categories)
            {
                //Категория WTG
                WtgCategory wtgC = new();
                wtgC.Index = cat.Id;
                wtgC.Name = cat.Name;
                wtg.Categories.Add(wtgC);

                //Триггеры
                foreach (ITrigger itrg in cat.Triggers)
                {
                    //Триггер WTG
                    WtgTrigger wtgTrg = new WtgTrigger();
                    wtgTrg.Name = itrg.Name;
                    wtgTrg.Description = itrg.Description;
                    wtgTrg.CategoryId = cat.Id;
                    if (itrg is Trigger trg)
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
                    else if (itrg is TriggerComment com)
                    {
                        wtgTrg.IsComment = true;
                    }
                    wtg.Triggers.Add(wtgTrg);

                    //Код WCT
                    string code = "";
                    if (itrg is Trigger)
                    {
                        code = ((Trigger)itrg).Code;
                    }
                    wct.CustomTexts.Add(code);
                }
            }

            //Сохранение в архив
            wtg.SaveToMPQ(mpq);
            wct.SaveToMPQ(mpq);
        }
    }
}