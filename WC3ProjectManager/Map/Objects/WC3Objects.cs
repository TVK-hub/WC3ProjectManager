using System.Globalization;
using WC3Files;
using WC3Files.Common.Ini;
using WC3Files.Common.SLK;
using WC3Files.Metadata;
using WC3Files.Metadata.Objects;
using WC3Files.Objects;
using WC3ProjectManager.Map.Strings;
namespace WC3ProjectManager.Map.Objects
{
    public class WC3Objects : Dictionary<string, WC3Object>
    {
        //Тип
        public W3objTypes Type;

        //Версия игры
        public string GameVers
        {
            get;
            private set;
        }

        //Метаданные
        internal ObjMetadata Metadata
        {
            get
            {
                ObjMetadata meta;
                WC3Metadata metadata = WC3Metadata.GetByVers(GameVers);
                switch (Type)
                {
                    case W3objTypes.Unit:
                    case W3objTypes.Item:
                        meta = metadata.Units;
                        break;
                    case W3objTypes.Ability:
                        meta = metadata.Abilities;
                        break;
                    case W3objTypes.Buff:
                        meta = metadata.Buffs;
                        break;
                    case W3objTypes.Upgrade:
                        meta = metadata.Upgrades;
                        break;
                    case W3objTypes.Doodad:
                        meta = metadata.Doodads;
                        break;
                    case W3objTypes.Destructable:
                        meta = metadata.Destructables;
                        break;
                    default:
                        throw new Exception("Неизвестный тип объекта.");
                }
                return meta;
            }
        }

        //Конструктор
        public WC3Objects(W3objTypes type, string gameVer)
        {
            Type = type;
            GameVers = gameVer;
        }

        //Обновить
        public void UpdateFromW3slk(string slkPath, bool baseTable=false)
        {
            SlkFile slk = SlkFile.Load(slkPath);
            string slkName = Path.GetFileNameWithoutExtension(slkPath);
            WC3ObjectsUpdater upd = new(this);
            upd.FromW3slk(slk, slkName, baseTable);
        }
        public void UpdateFromW3ini(string iniPath)
        {
            IniFile ini = IniFile.Load(iniPath);
            WC3ObjectsUpdater upd = new(this);
            upd.FromW3ini(ini);
        }
        public void UpdateFromW3obj(W3objFile w3obj)
        {
            WC3ObjectsUpdater upd = new(this);
            upd.FromW3obj(w3obj);
        }
        public void UpdateFromPMini(string dir)
        {
            WC3ObjectsUpdater upd = new(this);
            upd.FromPMini(dir);
        }

        //Сохранить
        public void SaveToPMini(string dir, WC3Strings strings)
        {
            foreach (var obj in Values)
            {
                //Путь
                string path = dir;

                //Раса
                if (obj.Fields.TryGetByName("race", out WC3ObjectField? fRace))
                {
                    string race = (string)fRace.Value;
                    race = char.ToUpper(race[0]) + race[1..];
                    path += $"\\{race}";
                }

                //Каталог
                if (obj.Fields.TryGetByName("isbldg", out WC3ObjectField? fBuilding) && (int)fBuilding.Value == 1)
                {
                    path += "\\Buildings";
                }
                else if (obj.Fields.TryGetByName("campaign", out WC3ObjectField? fCampaign) && (int)fCampaign.Value == 1)
                {
                    path += "\\Campaign";
                }
                else if (obj.Fields.TryGetByName("special", out WC3ObjectField? fSpecial) && (int)fSpecial.Value == 1)
                {
                    path += "\\Special";
                }
                else if (obj.Fields.TryGetByName("hero", out WC3ObjectField? fHero) && (int)fHero.Value == 1)
                {
                    path += "\\Heroes";
                }
                else if (Type == W3objTypes.Unit && char.IsUpper(obj.Id[0]))
                {
                    path += "\\Heroes";
                }
                else if (obj.Fields.TryGetByName("item", out WC3ObjectField? fItem) && (int)fItem.Value == 1)
                {
                    path += "\\Items";
                }

                //Название
                path += $"\\[{obj.Id}]";
                if (!string.IsNullOrEmpty(obj.Name))
                {
                    string name = strings.Resolve(obj.Name);
                    path += $" {name.ToFileName()}";
                }
                path += ".ini";

                //Файл
                IniFile ini = new IniFile();

                //Записи
                int nameMaxLen = 0;
                Dictionary<IniEntry, WC3ObjectField> fieldsDict = new();
                WC3ObjectField? fLvls = obj.Fields.GetByName("levels");
                if (fLvls==null) fLvls = obj.Fields.GetByName("numVar");
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
                            v = strings.Resolve(f.Value.ToString());
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
                        int i1 = lay1.Index*100 + f1.Level;
                        int i2 = lay2.Index*100 + f2.Level;
                        return i1.CompareTo(i2);
                    });
                }

                //Сохранение
                string? pathDir = Path.GetDirectoryName(path);
                if (pathDir != null) Directory.CreateDirectory(pathDir);
                ini.Save(path);
            }
        }

        //В модификацию
        public W3objFile ToW3obj()
        {
            W3objFile w3obj = new();
            WC3Objects baseObjects = WC3ObjectsSet.Base[GameVers][Type];
            foreach (string objId in Keys)
            {
                //Объект
                WC3Object obj = this[objId];
                bool isCustom = !string.IsNullOrEmpty(obj.BaseId);
                WC3Object objBase = baseObjects[(!isCustom) ? obj.Id : obj.BaseId];

                //Поля
                List<W3objField> modFields = new();
                foreach (var key in obj.Fields.Keys)
                {
                    WC3ObjectField? f = obj.Fields.GetByIdLvl(key.Id, key.Level);
                    if (string.IsNullOrEmpty(f.Id)) { continue; }
                    WC3ObjectField? fBase = objBase.Fields.GetByIdLvl(key.Id, key.Level);
                    if (fBase == null || f.Value.ToString().Trim() != fBase.Value.ToString().Trim())
                    {
                        //Метаданные
                        ObjMetadataRecord meta = Metadata.Records[f.Id];

                        //Изменение
                        W3objField modField = new();
                        modField.Id = f.Id;
                        modField.Level = f.Level;
                        modField.DataPointer = meta.Data;

                        //Значение
                        modField.ValueType = W3objValueTypeEx.FromMetaType(meta.Type);
                        string? vStr = f.Value?.ToString();
                        switch (modField.ValueType)
                        {
                            case W3objValueType.Int:
                                {
                                    int v; if (!int.TryParse(vStr, out v))
                                    {
                                        meta.Fields.TryGetValue("minVal", out ObjMetadataField? fMin);
                                        vStr = fMin.Value.ToString();
                                        v = int.Parse(vStr);
                                    }
                                    modField.Value = v;
                                }
                                break;
                            case W3objValueType.Real:
                            case W3objValueType.Unreal:
                                {
                                    float v; if (!float.TryParse(vStr, CultureInfo.InvariantCulture, out v))
                                    {
                                        meta.Fields.TryGetValue("minVal", out ObjMetadataField? fMin);
                                        vStr = fMin?.Value?.ToString();
                                        v = float.Parse(vStr);
                                    }
                                    modField.Value = v;
                                }
                                break;
                            case W3objValueType.String:
                                modField.Value = vStr;
                                break;
                        }

                        //В список
                        modFields.Add(modField);
                    }
                }

                //Модификация
                if (modFields.Count > 0)
                {
                    W3objMod mod = new W3objMod();
                    mod.Fields.AddRange(modFields);
                    if (!isCustom)
                    {
                        mod.BaseId = obj.Id;
                        w3obj.StandartMods.Add(mod);
                    }
                    else
                    {
                        mod.BaseId = obj.BaseId;
                        mod.CustomId = obj.Id;
                        w3obj.CustomMods.Add(mod);
                    }
                }
            }
            w3obj.Version = 2;
            return w3obj;
        }

        //Клонировать
        public WC3Objects Clone()
        {
            WC3Objects clone = new WC3Objects(Type, GameVers);
            foreach (var item in this)
            {
                clone[item.Key] = item.Value.Clone();
            }
            return clone;
        }
    }
}