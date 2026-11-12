using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WC3Files.Triggers;

namespace WC3Files.Common.Ini
{
    public class IniFile
    {
        //Записи
        public IEnumerable<IniEntry> Entries
        {
            get
            {
                return Sections.SelectMany(s => s.Entries);
            }
        }

        //Секции
        public List<IniSection> Sections
        {
            get;
        } = new List<IniSection>();

        //Загрузка
        public static IniFile Load(string path)
        {
            IniReader r = new IniReader(path);
            return r.ReadFile();
        }
        public static IniFile[] LoadFromDir(string dir, bool subfolders = false)
        {
            //Файлы
            FileInfo[] files;
            if (Directory.Exists(dir))
            {
                SearchOption opt = (subfolders) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                files = new DirectoryInfo(dir).GetFiles("*.ini", opt);
            }
            else
            {
                files = new FileInfo[0];
            }

            //Ini
            IniFile[] ini = new IniFile[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                ini[i] = Load(files[i].FullName);
            }
            return ini;
        }

        //Сохранение
        public void Save(string path)
        {
            using (IniWriter writer = new IniWriter(path))
            {
                writer.WriteFile(this);
            }
        }
    }
}
