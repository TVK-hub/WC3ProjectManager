using System.Globalization;
using WC3Files.Common.Ini;
using WC3Files.Metadata;
using WC3Files.Metadata.Objects;
using WC3Files.Objects;
using WC3ProjectManager.Project;

namespace WC3ProjectManager.Map.Objects
{
    public class WC3ObjectSerializer
    {
        public static void Save(WC3Object obj, string path)
        {
            //Файл
            IniFile ini = new IniFile();

            //Записи
            int nameMaxLen = 0;
            Dictionary<IniEntry, WC3ObjectField> fieldsDict = new();
            WC3ObjectField? fLvls = obj.Fields.GetByName("levels");
            if (fLvls == null) fLvls = obj.Fields.GetByName("numVar");
            foreach (WC3ObjectField f in obj.Fields.Values)
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
        public static WC3Object? Load(string path)
        {
            IniFile ini = IniFile.Load(path);
            return Load(ini);
        }
        public static WC3Object? Load(IniFile ini)
        {
            //Поля
            string? objId = null;
            string? objBaseId = null;
            string? objParentId = null;
            W3objTypes? objType = null;
            List<WC3ObjectField> fields = new();
            Dictionary<string, int> idsCount = new();
            foreach (IniEntry e in ini.Entries)
            {
                //Поле
                WC3ObjectField f = new WC3ObjectField();
                fields.Add(f);

                //Id
                string id = e.Name;
                int inx = id.IndexOf('|');
                if (inx > 0) { id = id[..inx]; }
                id = id.Trim();
                f.Id = id;

                //Мета
                ObjMetadataRecord? meta = null;
                WC3Metadata metadata = WC3Metadata.GetByVers(WC3Project.Current.BaseBuild.GameVersion);
                if (objType!=null && metadata.Objects.TryGetValue((W3objTypes)objType, out ObjMetadata? metaLst))
                {
                    metaLst.Records.TryGetValue(id, out meta);
                }

                //Имя
                if (meta != null)
                {
                    f.Name = meta.Name;
                }
                else
                {
                    f.Name = id;
                    f.Id = "";
                }

                //Уровень
                int count; idsCount.TryGetValue(id, out count);
                if (meta != null && (int?)meta.GetFieldValue("repeat") > 0)
                {
                    f.Level = count + 1;
                }
                else
                {
                    f.Level = count;
                }
                idsCount[id] = count + 1;

                //Значение
                f.Value = e.Value.Trim();

                //Специальное поле
                if (id == "ID")
                {
                    objId = f.Value.ToString();
                }
                else if (id == "ID_Base")
                {
                    objBaseId = f.Value.ToString();
                }
                else if (id == "ID_Parent")
                {
                    objParentId = f.Value.ToString();
                }
                else if (id == "Type")
                {
                    objType = W3objTypesEx.Parse(f.Value.ToString());
                }
            }

            //Объект
            WC3Object? obj = null;
            if (objId != null)
            {
                //Новый объект
                obj = new WC3Object();
                obj.WC3Type = (W3objTypes)objType;

                //Добавить поля
                foreach (var f in fields)
                {
                    obj.Fields[f.Id, f.Level] = f;
                }
            }
            return obj;
        }
    }
}
