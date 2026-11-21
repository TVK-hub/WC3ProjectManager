using Microsoft.VisualBasic;
using System.Globalization;
using WC3Files.Common.Ini;
using WC3Files.Metadata;
using WC3Files.Metadata.Objects;
using WC3Files.Objects;
using WC3ProjectManager.Project;

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
                return W3objTypesEx.Parse(WC3TypeStr);
            }
            set
            {
                WC3TypeStr = W3objTypesEx.ToString(value);
            }
        }
        public string WC3TypeStr
        {
            get
            {
                string str = "";
                var f = Fields.GetByName("Type");
                if (f != null) { str = (string)f.Value; }
                return str;
            }
            set
            {
                var f = Fields.GetByName("Type");
                if (f == null) { Fields.Create("", "Type", value, 0); }
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

        //Сериализация
        public void Save(string path)
        {
            WC3ObjectSerializer.Save(this, path);
        }
        public static WC3Object Load(string path)
        {
            return WC3ObjectSerializer.Load(path);
        }

        //В модификатор
        public W3objMod? ToMod(WC3Objects objs)
        {
            //Метаданные
            string gameVer = WC3Project.Current.BaseBuild.GameVersion;
            ObjMetadata metadata = WC3Metadata.GetByVers(gameVer).Objects[WC3Type];

            //Есть родитель?
            bool hasParent = !string.IsNullOrEmpty(ParentId);
            WC3Object? parent = (hasParent) ? objs[ParentId] : null;

            //Кастомный объект?
            bool isCustom = hasParent || !string.IsNullOrEmpty(BaseId);

            //Базовый объект
            WC3Object baseObj;
            WC3Objects baseObjects = WC3ObjectsSet.Base[gameVer][WC3Type];
            if (isCustom)
            {
                if (!hasParent)
                {
                    baseObj = baseObjects[BaseId];
                }
                else
                {
                    baseObj = baseObjects[(!string.IsNullOrEmpty(parent.BaseId)) ? parent.BaseId : parent.Id];
                }
            }
            else
            {
                baseObj = baseObjects[Id];
            }

            //Поля
            List<W3objField> modFields = new();
            ToMod_AddFields(this, baseObj, modFields, metadata);
            if (hasParent){ ToMod_AddFields(parent, baseObj, modFields, metadata); }

            //Модификация
            W3objMod? mod = null;
            if (modFields.Count > 0)
            {
                mod = new W3objMod();
                mod.Fields.AddRange(modFields);
                if (!isCustom)
                {
                    mod.BaseId = Id;
                }
                else
                {
                    mod.BaseId = baseObj.Id;
                    mod.CustomId = Id;
                }
            }
            return mod;
        }
        private void ToMod_AddFields(WC3Object source, WC3Object baseObj, List<W3objField> modFields, ObjMetadata metadata)
        {
            foreach (var key in source.Fields.Keys)
            {
                //Текущее поле
                WC3ObjectField? f = source.Fields.GetByIdLvl(key.Id, key.Level);
                if (string.IsNullOrEmpty(f.Id)) { continue; }
                if (this != source && Fields.Keys.Contains(key)) { continue; }

                //Базовое поле
                WC3ObjectField? fBase = baseObj.Fields.GetByIdLvl(key.Id, key.Level);

                //Значения различаются?
                if (fBase == null || f.Value.ToString().Trim() != fBase.Value.ToString().Trim())
                {
                    //Метаданные
                    ObjMetadataRecord meta = metadata.Records[f.Id];

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