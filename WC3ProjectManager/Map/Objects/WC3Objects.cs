using Microsoft.VisualBasic;
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
        public void UpdateFromW3slk(string slkPath, WC3Strings strings, bool baseTable=false)
        {
            SlkFile slk = SlkFile.Load(slkPath);
            string slkName = Path.GetFileNameWithoutExtension(slkPath);
            WC3ObjectsUpdater upd = new(this, strings);
            upd.FromW3slk(slk, slkName, baseTable);
        }
        public void UpdateFromW3ini(string iniPath, WC3Strings strings)
        {
            IniFile ini = IniFile.Load(iniPath);
            WC3ObjectsUpdater upd = new(this, strings);
            upd.FromW3ini(ini);
        }
        public void UpdateFromW3obj(W3objFile w3obj, WC3Strings strings)
        {
            WC3ObjectsUpdater upd = new(this, strings);
            upd.FromW3obj(w3obj);
        }
        public void UpdateFromPMini(string dir)
        {
            WC3ObjectsUpdater upd = new(this, null);
            upd.FromPMini(dir);
        }

        //Сохранить
        public void SaveToPMini(string dir)
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
                    string name = obj.Name;
                    path += $" {name.ToFileName()}";
                }
                path += ".ini";

                //Сохранить в файл
                obj.Save(path);
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
                        if (Type == W3objTypes.Unit && f.Id == "udp1" && f.Value.ToString().IndexOf(",") > 0) { }
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
                                    vStr = vStr.Replace(',', '.');
                                    float v; if (!float.TryParse(vStr, CultureInfo.InvariantCulture, out v))
                                    {
                                        meta.Fields.TryGetValue("minVal", out ObjMetadataField? fMin);
                                        vStr = fMin?.Value?.ToString();
                                        v = float.Parse(vStr.Replace(',', '.'));
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