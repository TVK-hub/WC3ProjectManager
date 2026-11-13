using WC3Files.Common.Ini;
using WC3Files.Common.MPQ;
using WC3ProjectManager.Map.Objects;
using WC3ProjectManager.Map.Strings;
namespace WC3ProjectManager.Map.Config
{
    public class WC3Config
    {
        //Файлы
        public IniFile? Misc
        {
            private set;
            get;
        }
        public IniFile? Skin
        {
            private set;
            get;
        }

        //Загрузка
        public static WC3Config LoadFromMPQ(MpqArchive mpq, WC3Strings strings = null)
        {
            //Объект
            WC3Config cf = new WC3Config();

            //Файлы
            cf.Misc = IniFile.LoadFromMPQ(mpq, "war3mapMisc.txt");
            cf.Skin = IniFile.LoadFromMPQ(mpq, "war3mapSkin.txt");
            if (cf.Misc != null) cf.Misc.SpacesAroundDelimiter = false;
            if (cf.Skin != null) cf.Skin.SpacesAroundDelimiter = false;
            if (cf.Skin != null && strings != null)
            {
                foreach (var en in cf.Skin.Entries)
                {
                    en.Value = strings.Resolve(en.Value);
                }
            }
            return cf;
        }
        public static WC3Config LoadFromDir(string dir)
        {
            //Объект
            WC3Config cf = new WC3Config();

            //Файлы
            if (File.Exists(dir + "\\" + "war3mapMisc.txt")) cf.Misc = IniFile.Load(dir + "\\" + "war3mapMisc.txt");
            if (File.Exists(dir + "\\" + "war3mapSkin.txt")) cf.Skin = IniFile.Load(dir + "\\" + "war3mapSkin.txt");
            if (cf.Misc != null) cf.Misc.SpacesAroundDelimiter = false;
            if (cf.Skin != null) cf.Skin.SpacesAroundDelimiter = false;
            return cf;
        }

        //Сохранить
        public void SaveToMPQ(MpqArchive mpq)
        {
            string miscPath = "war3mapMisc.txt";
            string skinPath = "war3mapSkin.txt";
            if (Misc != null) { Misc.SaveToMPQ(mpq, miscPath); } else { mpq.DeleteFile(miscPath); }
            if (Skin != null) { Skin.SaveToMPQ(mpq, skinPath); } else { mpq.DeleteFile(skinPath); }
        }
        public void SaveToProject(string dir)
        {
            Misc?.Save(dir+"\\war3mapMisc.txt");
            Skin?.Save(dir+"\\war3mapSkin.txt");
        }

        //Внедрить
        public void Inject(WC3Config src)
        {
            void injectIni(IniFile fThis, IniFile f)
            {
                foreach (IniSection sec in f.Sections)
                {
                    //Секция
                    IniSection? secThis = fThis.Sections.Find(itm=>itm.Name==sec.Name);
                    if (secThis == null) { secThis = new(sec.Name); fThis.Sections.Add(secThis); }

                    //Записи
                    foreach (IniEntry en in sec.Entries)
                    {
                        if (!secThis.Entries.Exists(itm => itm.Name == en.Name))
                        {
                            secThis.Entries.Add(en.Clone(secThis));
                        }
                    }
                }
            }
            if (Misc == null) { Misc = src.Misc?.Clone(); } else if (src.Misc != null) { injectIni(Misc, src.Misc); }
            if (Skin == null) { Skin = src.Skin?.Clone(); } else if (src.Skin != null) { injectIni(Skin, src.Skin); }
        }

        //Сброс
        public void Reset()
        {
            Misc = null;
            Skin = null;
        }

        //Клонировать
        public WC3Config Clone()
        {
            WC3Config clone = new();
            clone.Misc = Misc?.Clone();
            clone.Skin = Skin?.Clone();
            return clone;
        }
    }
}
