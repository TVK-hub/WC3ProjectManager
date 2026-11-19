using Microsoft.VisualBasic;
using System.Globalization;
using WC3Files.Common.Ini;
using WC3Files.Objects;

namespace WC3ProjectManager.Map.Objects
{
    public class WC3Object
    {
        //Id
        public string Id
        {
            get
            {
                return (string)Fields.GetByName("ID").Value;
            }
            set
            {
                var f = Fields.GetByName("ID");
                f.Value = value;
            }
        }

        //Базовый Id
        public string BaseId
        {
            get
            {
                string id = "";
                var f = Fields.GetByName("ID_Base");
                if (f!=null) { id = f.Value.ToString(); }
                return id;
            }
            set
            {
                var f = Fields.GetByName("ID_Base");
                if (f == null) { Fields.Create("", "ID_Base", value, 0); }
                else { f.Value = value; }
            }
        }

        //Родительский Id
        public string ParentId
        {
            get
            {
                string id = "";
                var f = Fields.GetByName("ID_Parent");
                if (f != null) { id = f.Value.ToString(); }
                return id;
            }
            set
            {
                var f = Fields.GetByName("ID_Parent");
                if (f == null) { Fields.Create("", "ID_Parent", value, 0); }
                else { f.Value = value; }
            }
        }

        //Тип объекта
        public W3objTypes WC3Type
        {
            get
            {
                W3objTypes t = W3objTypes.All;
                var f = Fields.GetByName("Type");
                if (f != null) { t = W3objTypesEx.Parse((string)f.Value); }
                return t;
            }
            set
            {
                var f = Fields.GetByName("Type");
                if (f == null) { Fields.Create("", "Type", W3objTypesEx.ToString(value), 0); }
                else { f.Value = value; }
            }
        }

        //Имя
        public string Name
        {
            get
            {
                //Поле
                WC3ObjectField? f = Fields.GetByName("Name");
                if (f==null) { f = Fields.GetByName("EditorName"); }
                if (f==null) { f = Fields.GetByName("Bufftip"); }

                //Значение
                string s;
                if (f != null)
                {
                    s = f.Value?.ToString();
                }
                else
                {
                    s = "";
                }
                return s;
            }
            set
            {
                Fields["Name", 0].Value = value;
            }
        }

        //Суффикс
        public string EditorSuffix
        {
            get
            {
                //Поле
                WC3ObjectField? f = Fields.GetByName("EditorSuffix");

                //Значение
                string s;
                if (f != null)
                {
                    s = f.Value?.ToString();
                }
                else
                {
                    s = "";
                }
                return s;
            }
            set
            {
                Fields["EditorSuffix", 0].Value = value;
            }
        }

        //Поля
        public WC3ObjectFields Fields
        {
            get;
            private set;
        } = new();

        //Конструктор
        public WC3Object()
        {

        }

        //Сохранить
        public void Save(string path)
        {
            //Файл
            IniFile ini = new IniFile();

            //Записи
            int nameMaxLen = 0;
            Dictionary<IniEntry, WC3ObjectField> fieldsDict = new();
            WC3ObjectField? fLvls = Fields.GetByName("levels");
            if (fLvls == null) fLvls = Fields.GetByName("numVar");
            foreach (WC3ObjectField f in Fields.Values)
            {
                //Лишний уровень?
                if (fLvls != null && f.Level > (int)fLvls.Value) { continue; }

                //Секция
                WC3ObjectFieldLayout layout = WC3ObjectLayout.GetFieldLayout(f);
                IniSection? sec = ini.Sections.FirstOrDefault(itm => itm.Name == layout.SectionName);
                if (sec == null)
                {
                    sec = new IniSection(layout.SectionName);
                    ini.Sections.Add(sec);
                }

                //Запись
                IniEntry e = new IniEntry(sec); fieldsDict[e] = f;
                sec.Entries.Add(e);

                //Имя
                if (!string.IsNullOrEmpty(layout.Name))
                {
                    e.Name = layout.Name;
                }
                else
                {
                    e.Name = f.Name;
                }
                if (f.Level > 0) { e.Name += " " + f.Level; }
                if (!string.IsNullOrEmpty(f.Id)) { e.Name = f.Id + " | " + e.Name; }
                nameMaxLen = Math.Max(nameMaxLen, e.Name.Length);

                //Значение
                string v = "_";
                if (f.Value != null)
                {
                    Type type = f.Value.GetType();
                    if (type == typeof(float))
                    {
                        float real = (float)f.Value;
                        v = real.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        v = f.Value.ToString();
                    }
                }
                e.Value = v;
            }

            //Отступы
            foreach (IniEntry e in ini.Entries)
            {
                while (e.Name.Length < nameMaxLen)
                {
                    e.Name += " ";
                }
            }

            //Сортировка
            ini.Sections.Sort(
                delegate (IniSection sec1, IniSection sec2)
                {
                    var lay1 = WC3ObjectLayout.GetFieldLayout(fieldsDict[sec1.Entries[0]]);
                    var lay2 = WC3ObjectLayout.GetFieldLayout(fieldsDict[sec2.Entries[0]]);
                    return lay1.SectionIndex.CompareTo(lay2.SectionIndex);
                }
            );
            foreach (IniSection sec in ini.Sections)
            {
                sec.Entries.Sort(delegate (IniEntry e1, IniEntry e2) {
                    WC3ObjectField f1 = fieldsDict[e1];
                    WC3ObjectField f2 = fieldsDict[e2];
                    var lay1 = WC3ObjectLayout.GetFieldLayout(f1);
                    var lay2 = WC3ObjectLayout.GetFieldLayout(f2);
                    int i1 = lay1.Index * 100 + f1.Level;
                    int i2 = lay2.Index * 100 + f2.Level;
                    return i1.CompareTo(i2);
                });
            }

            //Сохранение
            string? pathDir = Path.GetDirectoryName(path);
            if (pathDir != null) Directory.CreateDirectory(pathDir);
            ini.Save(path);
        }

        //Клонировать
        public WC3Object Clone()
        {
            WC3Object clone = new();
            foreach (var f in Fields.Values)
            {
                clone.Fields[f.Id, f.Level] = f.Clone();
            }
            return clone;
        }

        //В строку
        public override string ToString()
        {
            return Name;
        }
    }
}