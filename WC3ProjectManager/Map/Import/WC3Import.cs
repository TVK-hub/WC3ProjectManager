using WC3Files;
using WC3Files.Common.Ini;
using WC3Files.Common.MPQ;
namespace WC3ProjectManager.Map.Import
{
    public class WC3Import
    {
        //Версия
        public int StructVersion
        {
            set;
            get;
        }

        //Записи
        public List<WC3ImportRecord> Records
        {
            get;
        } = new();

        //Конструктор
        public WC3Import()
        {

        }

        //Внедрить
        public void Inject(WC3Import src)
        {
            foreach (WC3ImportRecord recSrc in src.Records)
            {
                //Запись
                WC3ImportRecord? r = Records.Find(itm=>itm.ImportPath == recSrc.ImportPath);
                if (r == null)
                {
                    r = new WC3ImportRecord();
                    r.ImportPath = recSrc.ImportPath;
                    Records.Add(r);
                }

                //Данные
                r.FileData = recSrc.FileData;

                //Путь
                r.DiscPath = recSrc.DiscPath;
            }
        }

        //Очистить
        public void Clear()
        {
            Records.Clear();
        }

        //Загрузка
        public static WC3Import LoadFromMPQ(MpqArchive mpq)
        {
            //Объект
            WC3Import import = new WC3Import();

            //Список
            ImpFile imp = ImpFile.LoadFromMPQ(mpq);
            import.StructVersion = imp.Version;

            //Записи
            for (int i = 0; i < imp.Entries.Count; i++)
            {
                //Запись
                WC3ImportRecord rec = new();
                
                //Путь к файлу внутри карты
                ImpEntry impEn = imp.Entries[i];
                rec.ImportPath = impEn.GetFullPath();
                rec.DiscPath = rec.ImportPath;

                //Данные файла
                using (MpqFile f = mpq.OpenFile(rec.ImportPath))
                {
                    if (f!=null) { rec.FileData = f.Read(); }
                    else { rec.FileData = new byte[0]; }
                }

                //В список
                import.Records.Add(rec);
            }
            return import;
        }
        public static WC3Import LoadFromDir(string dir)
        {
            //Импорт
            WC3Import imp = new WC3Import();

            //ini
            IniFile[] files = IniFile.LoadFromDir(dir);
            foreach (IniFile f in files)
            {
                //Записи
                foreach (var en in f.Entries)
                {
                    WC3ImportRecord r = new WC3ImportRecord();
                    r.DiscPath = en.Name;
                    r.ImportPath = en.Value;
                    r.FileData = File.ReadAllBytes(dir + "\\" + r.DiscPath);
                    imp.Records.Add(r);
                }
            }
            return imp;
        }

        //Сохранить
        public void SaveToMPQ(MpqArchive mpq)
        {
            //Чистка
            ImpFile imp = ImpFile.LoadFromMPQ(mpq);
            foreach (ImpEntry en in imp.Entries)
            {
                string path = en.GetFullPath();
                if (Records.Exists(r=>r.ImportPath == path) == false)
                {
                    //Удалить файл
                    mpq.DeleteFile(path);
                }
            }

            //Запись
            imp = new ImpFile();
            imp.Version = 1;
            foreach (WC3ImportRecord rec in Records)
            {
                //Запись в .imp
                ImpEntry en = new ImpEntry();
                en.SetFullPath(rec.ImportPath);
                imp.Entries.Add(en);

                //Запись файла в архив
                mpq.CreateFile(rec.ImportPath, rec.FileData);
            }

            //Сохранение в архив
            imp.SaveToMPQ(mpq);
        }
        public void SaveToDir(string dir)
        {
            //Файлы
            for (int i = 0; i < Records.Count; i++)
            {
                //Запись
                WC3ImportRecord rec = Records[i];

                //Сохранить файл
                rec.SaveToFile(dir+"\\"+rec.ImportPath);
            }

            //Import.ini
            using (StreamWriter writer = new StreamWriter(dir + "\\" + "Import.ini"))
            {
                foreach (WC3ImportRecord rec in Records)
                {
                    writer.WriteLine(rec.ImportPath + " = " + rec.ImportPath);
                }
            }
        }
    }
}
