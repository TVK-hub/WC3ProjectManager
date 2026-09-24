using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WC3Files.Triggers;

namespace WC3Files.Ini
{
    public class IniFile
    {
        //Записи
        public List<IniEntry> Entries
        {
            get;
        } = new List<IniEntry>();

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
            SearchOption opt = (subfolders) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            FileInfo[] files = new DirectoryInfo(dir).GetFiles("*.ini", opt);

            //Ini
            IniFile[] ini = new IniFile[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                ini[i] = Load(files[i].FullName);
            }
            return ini;
        }
    }
}
