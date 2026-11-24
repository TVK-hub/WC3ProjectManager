using WC3Files;
using WC3Files.Common.MPQ;
using WC3Files.Objects;
using WC3ProjectManager.Map.Strings;
using static System.Runtime.InteropServices.JavaScript.JSType;
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
        public static WC3ObjectsSet LoadFromMPQ(MpqArchive mpq, string gameVer, WC3Strings strings)
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
                    objs[objType].UpdateFromW3obj(file, strings);
                }
            }
            return objs;
        }
        public static WC3ObjectsSet LoadFromDirs(IEnumerable<string> dirs, string gameVer)
        {
            //Сет
            WC3ObjectsSet objSet = new(gameVer);

            //Файлы
            foreach (var dir in dirs)
            {
                SearchOption opt = SearchOption.AllDirectories;
                FileInfo[] files = new DirectoryInfo(dir).GetFiles("*.ini", opt);
                foreach (FileInfo file in files)
                {
                    //Загрузка
                    WC3Object obj = WC3Object.Load(file.FullName);

                    //Добавить в коллекцию
                    if (!objSet.TryGetValue(obj.WC3Type, out WC3Objects objs))
                    {
                        objs = new WC3Objects(obj.WC3Type, gameVer);
                        objSet[obj.WC3Type] = objs;
                    }
                    objs[obj.Id] = obj;
                }
            }
            return objSet;
        }

        //Сохранить
        public void SaveToProject(string dir)
        {
            foreach (var itm in this)
            {
                WC3Objects objs = itm.Value;
                foreach (WC3Object obj in objs.Values)
                {
                    //Путь
                    string path = dir;
                    switch (obj.WC3Type)
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
                    else if (obj.WC3Type == W3objTypes.Unit && char.IsUpper(obj.Id[0]))
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

                    //Сохранение
                    obj.Save(path);
                }
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
                string path = pathes[objs.WC3Type];
                w3obj.SaveToMPQ(mpq, path, objs.WC3Type);
            }
        }

        //Внедрить
        public void Inject(WC3ObjectsSet src)
        {
            foreach (WC3Objects objs in src.Values)
            {
                foreach (WC3Object obj in objs.Values)
                {
                    this[objs.WC3Type][obj.Id] = obj;
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
                foreach (var item in baseObjects[objs.WC3Type])
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
