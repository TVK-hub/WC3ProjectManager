using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WC3Files.Common.MPQ;
using WC3Files.Objects;
using WC3Files.Triggers;
using static System.Net.Mime.MediaTypeNames;

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
            var lines = File.ReadLines(path);
            IniReader r = new IniReader(lines);
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
        public static IniFile? LoadFromMPQ(MpqArchive mpq, string path)
        {
            IniFile? ini = null;
            using (MpqFile f = mpq.OpenFile(path))
            {
                if (f != null)
                {
                    byte[] data = f.Read();
                    string[] lines = Regex.Split(Encoding.UTF8.GetString(data), "\r\n|\r|\n");
                    IniReader r = new IniReader(lines);
                    ini = r.ReadFile();
                }

            }
            return ini;
        }

        //Сохранение
        public void Save(string path, bool spacesDel=true)
        {
            //Директория
            string? dir = Path.GetDirectoryName(path);
            if (dir!=null && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
            
            //Файл
            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                IniWriter writer = new IniWriter(stream);
                writer.WriteFile(this, spacesDel);
                writer.Dispose();
            }
        }
        public void SaveToMPQ(MpqArchive mpq, string path, bool spacesDel=true)
        {
            byte[] data = null;
            using (MemoryStream stream = new MemoryStream())
            {
                IniWriter writer = new IniWriter(stream);
                writer.WriteFile(this, spacesDel);
                writer.Dispose();
                data = stream.ToArray();
            }
            mpq.CreateFile(path, data);
        }

        //Клонировать
        public IniFile Clone()
        {
            IniFile clone = new();
            foreach (IniSection sec in this.Sections)
            {
                IniSection secClone = sec.Clone();
                clone.Sections.Add(secClone);
            }
            return clone;
        }
    }
}
