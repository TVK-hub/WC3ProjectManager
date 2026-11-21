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
        public W3objTypes WC3Type;

        //Версия игры
        public string GameVers
        {
            get;
            private set;
        }

        //Конструктор
        public WC3Objects(W3objTypes type, string gameVer)
        {
            WC3Type = type;
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

        //В модификацию
        public W3objFile ToW3obj()
        {
            W3objFile w3obj = new();
            foreach (string objId in Keys)
            {
                W3objMod? mod = this[objId].ToMod(this);
                if (mod != null)
                {
                    if (!mod.IsCustom())
                    {
                        w3obj.StandartMods.Add(mod);
                    }
                    else
                    {
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
            WC3Objects clone = new WC3Objects(WC3Type, GameVers);
            foreach (var item in this)
            {
                clone[item.Key] = item.Value.Clone();
            }
            return clone;
        }
    }
}