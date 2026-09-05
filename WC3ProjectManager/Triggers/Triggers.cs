using System.Net.Http.Headers;
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