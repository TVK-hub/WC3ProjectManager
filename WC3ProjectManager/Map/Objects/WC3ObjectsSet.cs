using WC3Files.Common.MPQ;
using WC3Files.Objects;
using WC3ProjectManager.Map.Strings;
namespace WC3ProjectManager.Map.Objects
{
    public class WC3ObjectsSet : Dictionary<W3objTypes, WC3Objects>
    {
        /// <summary>
        /// Оригинальные объекты.
        /// Это сеты ванильных объектов, распределённых по версиям игры.
        /// </summary>
        public static WC3ObjectsSetBase Base
        {
            get;
            private set;
        } = new();

        //Версия игры
        public string GameVers
        {
            get;
            private set;
        }

        //Конструктор
        public WC3ObjectsSet(string gameVer)
        {
            GameVers = gameVer;
        }

        //Загрузка
        public static WC3ObjectsSet LoadFromMPQ(MpqArchive mpq, string gameVer)
        {
            WC3ObjectsSet baseObjects = Base[gameVer];
            WC3ObjectsSet objs = baseObjects.Clone();
            Dictionary<W3objTypes, string> pathes = new()
            {
                [W3objTypes.Unit]         = "war3map.w3u",
                [W3objTypes.Ability]      = "war3map.w3a",
                [W3objTypes.Item]         = "war3map.w3t",
                [W3objTypes.Buff]         = "war3map.w3h",
                [W3objTypes.Upgrade]      = "war3map.w3q",
                [W3objTypes.Doodad]       = "war3map.w3d",
                [W3objTypes.Destructable] = "war3map.w3b"
            };
            foreach (var itm in pathes)
            {
                //W3obj
                string fpath = itm.Value;
                W3objTypes objType = itm.Key;
                W3objFile? file = W3objFile.LoadFromMPQ(mpq, fpath, objType);

                //Обновление
                if (file != null)
                {
                    objs[objType].UpdateFromW3obj(file);
                }
            }
            return objs;
        }
        public static WC3ObjectsSet LoadFromDir(string dir, string gameVer)
        {
            WC3ObjectsSet objSet = new(gameVer);
            foreach (var t in Enum.GetValues<W3objTypes>())
            {
                //Путь
                if (t == W3objTypes.All) { continue; }
                string path = dir;
                switch (t)
                {
                    case W3objTypes.Unit:
                        path += "\\Units";
                        break;
                    case W3objTypes.Item:
                        path += "\\Items";
                        break;
                    case W3objTypes.Ability:
                        path += "\\Abilities";
                        break;
                    case W3objTypes.Buff:
                        path += "\\Buffs";
                        break;
                    case W3objTypes.Upgrade:
                        path += "\\Upgrades";
                        break;
                    case W3objTypes.Doodad:
                        path += "\\Doodads";
                        break;
                    case W3objTypes.Destructable:
                        path += "\\Destructable";
                        break;
                }

                //Объекты
                WC3Objects objs = new(t, gameVer);
                objs.UpdateFromPMini(path);
                objSet[t] = objs;
            }
            return objSet;
        }

        //Сохранить
        public void SaveToProject(string dir, WC3Strings strings)
        {
            foreach (var itm in this)
            {
                WC3Objects objs = itm.Value;
                string path = dir;
                switch (objs.Type)
                {
                    case W3objTypes.Unit:
                        path += "\\Units";
                        break;
                    case W3objTypes.Item:
                        path += "\\Items";
                        break;
                    case W3objTypes.Ability:
                        path += "\\Abilities";
                        break;
                    case W3objTypes.Buff:
                        path += "\\Buffs";
                        break;
                    case W3objTypes.Upgrade:
                        path += "\\Upgrades";
                        break;
                    case W3objTypes.Doodad:
                        path += "\\Doodads";
                        break;
                    case W3objTypes.Destructable:
                        path += "\\Destructable";
                        break;
                }
                objs.SaveToPMini(path, strings);
            }
        }
        public void SaveToMPQ(MpqArchive mpq)
        {
            Dictionary<W3objTypes, string> pathes = new()
            {
                [W3objTypes.Unit] = "war3map.w3u",
                [W3objTypes.Ability] = "war3map.w3a",
                [W3objTypes.Item] = "war3map.w3t",
                [W3objTypes.Buff] = "war3map.w3h",
                [W3objTypes.Upgrade] = "war3map.w3q",
                [W3objTypes.Doodad] = "war3map.w3d",
                [W3objTypes.Destructable] = "war3map.w3b"
            };
            foreach (WC3Objects objs in Values)
            {
                W3objFile w3obj = objs.ToW3obj();
                string path = pathes[objs.Type];
                w3obj.SaveToMPQ(mpq, path, objs.Type);
            }
        }

        //Внедрить
        public void Inject(WC3ObjectsSet src)
        {
            foreach (WC3Objects objs in src.Values)
            {
                foreach (WC3Object obj in objs.Values)
                {
                    this[objs.Type][obj.Id] = obj;
                }
            }
        }

        //Сброс
        public void Reset()
        {
            WC3ObjectsSet baseObjects = Base[GameVers];
            foreach (var objs in Values)
            {
                objs.Clear();
                foreach (var item in baseObjects[objs.Type])
                {
                    objs[item.Key] = item.Value.Clone();
                }
            }
        }

        //Клонировать
        public WC3ObjectsSet Clone()
        {
            WC3ObjectsSet clone = new WC3ObjectsSet(GameVers);
            foreach (var kv in this)
            {
                clone[kv.Key] = kv.Value.Clone();
            }
            return clone;
        }
    }
}
