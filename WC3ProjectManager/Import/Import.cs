using WC3Files;
using WC3Files.Ini;
using WC3Files.MPQ;
namespace WC3ProjectManager
{
    public class Import
    {
        //Версия
        public int StructVersion
        {
            set;
            get;
        }

        //Записи
        public List<ImportRecord> Records
        {
            get;
        } = new();

        //Конструктор
        public Import()
        {

        }

        //Внедрить
        public void Inject(Import src)
        {
            foreach (ImportRecord recSrc in src.Records)
            {
                //Запись
                ImportRecord? r = Records.Find(itm=>itm.ImportPath == recSrc.ImportPath);
                if (r == null)
                {
                    r = new ImportRecord();
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
        public void LoadFromMPQ(MpqArchive mpq)
        {
            //Список
            ImpFile imp = ImpFile.LoadFromMPQ(mpq);
            StructVersion = imp.Version;

            //Записи
            for (int i = 0; i < imp.Entries.Count; i++)
            {
                //Запись
                ImportRecord rec = new();
                
                //Путь к файлу внутри карты
                ImpEntry impEn = imp.Entries[i];
                rec.ImportPath = impEn.GetFullPath();
                rec.DiscPath = rec.ImportPath;

                //Данные файла
                using (MpqFile f = mpq.OpenFile(rec.ImportPath))
                {
                    if (f.Ptr != 0x00){ rec.FileData = f.Read(); }
                    else { rec.FileData = new byte[0]; }
                }

                //В список
                Records.Add(rec);
            }
        }
        public static Import LoadFromDir(string dir)
        {
            //Импорт
            Import imp = new Import();

            //ini
            IniFile[] files = IniFile.LoadFromDir(dir);
            foreach (IniFile f in files)
            {
                //Записи
                foreach (var en in f.Entries)
                {
                    ImportRecord r = new ImportRecord();
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
            imp.Version = StructVersion;
            foreach (ImportRecord rec in Records)
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
                ImportRecord rec = Records[i];

                //Сохранить файл
                rec.SaveToFile(dir+"\\"+rec.ImportPath);
            }

            //Import.ini
            using (StreamWriter writer = new StreamWriter(dir + "\\" + "Import.ini"))
            {
                foreach (ImportRecord rec in Records)
                {
                    writer.WriteLine(rec.ImportPath + " = " + rec.ImportPath);
                }
            }
        }
    }
}
