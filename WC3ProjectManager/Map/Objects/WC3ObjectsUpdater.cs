using System.Diagnostics;
using System.Security.Cryptography;
using System.Xml.Linq;
using WC3Files.Common.Ini;
using WC3Files.Common.SLK;
using WC3Files.Metadata.Objects;
using WC3Files.Objects;
using WC3ProjectManager.Project;
namespace WC3ProjectManager.Map.Objects
{
    public class WC3ObjectsUpdater
    {
        //Объекты
        WC3Objects Objs;

        //Конструктор
        public WC3ObjectsUpdater(WC3Objects objs)
        {
            Objs = objs;
        }

        //W3slk
        public void FromW3slk(SlkFile slk, string slkName, bool baseTable = false)
        {
            //Мета
            var metaLst = Objs.Metadata.SlkRecords[slkName];

            //Записи
            for (int r = 1; r < slk.RowsCount; r++)
            {
                //Id
                string? objId = (string?)slk[r, 0];
                if (objId == null) { continue; }

                //Объект
                WC3Object? obj;
                if (!Objs.TryGetValue(objId, out obj) && baseTable)
                {
                    obj = new WC3Object(Objs.Type);
                    obj.Fields["ID", 0] = new WC3ObjectField()
                    {
                        Name = "ID",
                        Value = objId
                    };
                }
                if (obj == null) { continue; }

                //Поля
                for (int c = 1; c < slk.ColumnsCount; c++)
                {
                    //Имя и уровень
                    int lvl;
                    string? fName = (string?)slk[0, c];
                    if (fName == null) { continue; }
                    fName = ParseFieldName(fName, obj, out lvl, metaLst);

                    //Метаданные
                    ObjMetadataRecord? meta = metaLst.GetByObject(obj, fName);
                    if (meta == null) { continue; }

                    //Id
                    string? id = meta.Id;
                    if (id == null) { continue; }

                    //Значение
                    object? value = slk[r, c];
                    if (value == null) { continue; }

                    //Поле
                    obj.Fields.Create(id, fName, value, lvl);
                }

                //Добавить в коллекции
                Objs[obj.Id] = obj;
            }
        }
        
        //W3ini
        public void FromW3ini(IniFile ini)
        {
            //Мета
            var metaLst = Objs.Metadata.SlkRecords["Profile"];

            //Записи
            foreach (var sec in ini.Sections)
            {
                //Id
                string objId = sec.Name;

                //Объект
                if (!Objs.TryGetValue(objId, out WC3Object? obj)) { continue; }

                //Поля
                for (int i=0; i < sec.Entries.Count; i++)
                {
                    //Запись
                    IniEntry ent = sec.Entries[i];

                    //Метаданные
                    ObjMetadataRecord? meta = metaLst.GetByObject(obj, ent.Name);
                    if (meta == null) { continue; }

                    //Парсинг
                    if ((int?)meta.GetFieldValue("appendIndex") > 0)
                    {
                        for (; i < sec.Entries.Count; i++)
                        {
                            //Запись
                            ent = sec.Entries[i];

                            //Имя и уровень
                            string name; int lvl;
                            if (ent.Name == meta.Name)
                            {
                                name = ent.Name;
                                lvl = 1;
                            }
                            else
                            {
                                name = ParseFieldName(ent.Name, obj, out lvl, metaLst); lvl++;
                            }
                            if (name != meta.Name) { i--; break; }

                            //Значение
                            string v = ent.Value.Trim('"');

                            //Поле
                            obj.Fields.Create(meta.Id, name, v, lvl);
                        }
                    }
                    else if ((int?)meta.GetFieldValue("repeat") > 0)
                    {
                        Dictionary<int, string> values = ParseFieldValue(ent.Value, meta);
                        foreach (var kv in values)
                        {
                            int lvl = kv.Key + 1;
                            string v = kv.Value;
                            obj.Fields.Create(meta.Id, ent.Name, v, lvl);
                        }
                    }
                    else
                    {
                        string name = ent.Name;
                        var metaRecs = metaLst.GetAllByObject(obj, name);
                        if (meta.Index == -1 || metaRecs.Count == 1)
                        {
                            obj.Fields.Create(meta.Id, name, ent.Value.Trim('"'), 0);
                        }
                        else
                        {
                            Dictionary<int, string> values = ParseFieldValue(ent.Value, meta);
                            foreach (var kv in values)
                            {
                                //Значение
                                string v = kv.Value;

                                //Индекс и уровень
                                int inx = kv.Key;
                                int lvl = 0;
                                if (inx > metaRecs.Count)
                                {
                                    lvl += inx/metaRecs.Count;
                                    inx -= (lvl)*metaRecs.Count;
                                }

                                //Поле
                                meta = metaRecs.Find(itm=>itm.Index == inx);
                                obj.Fields.Create(meta.Id, name, v, lvl);
                            }
                        }
                    }
                }
            }
        }

        //W3obj
        public void FromW3obj(W3objFile w3obj)
        {
            FromW3objList(w3obj.StandartMods, false);
            FromW3objList(w3obj.CustomMods, true);
        }
        public void FromW3objList(List<W3objMod> lst, bool custom)
        {
            foreach (W3objMod m in lst)
            {
                //Id
                string id = (!custom) ? m.BaseId : m.CustomId;

                //Объект
                if (!Objs.TryGetValue(id, out WC3Object? obj))
                {
                    WC3Objects baseObjects = WC3ObjectsSet.Base[Objs.GameVers][Objs.Type];
                    if (custom && baseObjects.TryGetValue(m.BaseId, out WC3Object? objBase))
                    {
                        obj = objBase.Clone();
                        obj.Id = id;
                        obj.Fields["ID_Base", 0] = new WC3ObjectField("ID_Base", m.BaseId);
                        Objs[id] = obj;
                    }
                }
                if (obj == null)
                {
                    continue;
                }

                //Поля
                foreach (W3objField f in m.Fields)
                {
                    //Поле
                    var fMeta = Objs.Metadata.Records[f.Id];
                    WC3ObjectField? objField; if (!obj.Fields.TryGetByIdLvl(fMeta.Id, f.Level, out objField))
                    {
                        obj.Fields.Create(fMeta.Id, fMeta.Name, f.Value, f.Level);
                    }
                    else
                    {
                        objField.Value = f.Value;
                    }
                }
            }
        }

        //PMini
        public void FromPMini(string dir)
        {
            var metaLst = Objs.Metadata;
            IniFile[] inis = IniFile.LoadFromDir(dir, true);
            foreach (IniFile ini in inis)
            {
                //Поля
                string? objId = null;
                string? objBaseId = null;
                List <WC3ObjectField> fields = new();
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
                    metaLst.Records.TryGetValue(id, out meta);

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
                    idsCount[id] = count+1;

                    //Значение
                    f.Value = e.Value.Trim();

                    //Поле - ID?
                    if (id == "ID")
                    {
                        objId = f.Value.ToString();
                    }
                    else if (id == "ID_Base")
                    {
                        objBaseId = f.Value.ToString();
                    }
                }

                //Объект
                if (objId != null)
                {
                    WC3Object? obj;
                    if (!Objs.TryGetValue(objId, out obj))
                    {
                        obj = new WC3Object(Objs.Type);
                        Objs.Add(objId, obj);
                    }
                    foreach(var f in fields)
                    {
                        obj.Fields[f.Id, f.Level] = f;
                    }
                }
            }
        }

        //Общие
        private string ParseFieldName(string fName, WC3Object? obj, out int lvl, ObjMetadataRecords metaLst)
        {
            lvl = 0;
            if (metaLst.GetByObject(obj, fName) == null)
            {
                int i = fName.Length - 1; while (i >= 0 && char.IsDigit(fName[i]))
                {
                    i--;
                }
                if (i < fName.Length - 1)
                {
                    lvl = int.Parse(fName.Substring(i + 1, fName.Length - i - 1));
                    fName = fName.Substring(0, i + 1);
                }
            }
            return fName;
        }
        private Dictionary<int, string> ParseFieldValue(string str, ObjMetadataRecord meta)
        {
            //Списки
            Dictionary<int, string> dict = new();

            //Разделитель
            char spl = ',';
            bool quotes = false;
            if (str.IndexOf('"') > -1)
            {
                spl = '"';
                quotes = true;
            }

            //Разбивка
            int i1 = 0;
            int i2 = 0;
            int inx = 0;
            while (i2 < str.Length)
            {
                //Индексы
                if (!quotes)
                {
                    i1 = i2;
                    i2 = str.IndexOf(spl, i1);
                }
                else
                {
                    i1 = str.IndexOf(spl, i2) + 1;
                    i2 = str.IndexOf(spl, i1);
                }
                if (i2 <= 0) { i2 = str.Length; }

                //Значение
                string v = str[i1..i2];
                i2++;

                //Запись
                dict[inx] = v;
                inx++;
            }

            //Возвращение
            return dict;
        }
    }
}
