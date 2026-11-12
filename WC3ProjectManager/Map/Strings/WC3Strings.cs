using WC3Files.Common.Ini;
using WC3Files.Common.MPQ;
using WC3Files.Strings;
using WC3ProjectManager.Map.Objects;
using WC3ProjectManager.Project;
namespace WC3ProjectManager.Map.Strings
{
    public class WC3Strings
    {
        /// <summary>
        /// Оригинальные строки.
        /// Это сеты ванильных строк, распределённых по версиям игры.
        /// </summary>
        public static WC3StringsBase Base
        {
            get;
            private set;
        } = new();

        //Словарь
        private Dictionary<string, string> Dict = new();

        //Решение
        public string Resolve(string str)
        {
            if (Dict.TryGetValue(str, out string? v)) { str = v; }
            return str;
        }

        //Обновление
        public void UpdateFromIni(string iniPath)
        {
            //Ini
            IniFile ini = IniFile.Load(iniPath);

            //Записи
            foreach (IniEntry e in ini.Entries)
            {
                Dict[e.Name] = e.Value;
            }
        }
        public void UpdateFromWts(WtsFile wts)
        {
            foreach (var kv in wts.Dict)
            {
                string name = kv.Key;
                string value = kv.Value;
                if (name.StartsWith("STRING "))
                {
                    name = "TRIGSTR_" + int.Parse(name["STRING ".Length..]).ToString("000");
                }
                Dict[name] = value;
            }
        }

        //Загрузка
        public static WC3Strings LoadFromMPQ(MpqArchive mpq, string gameVer)
        {
            //wts
            WtsFile wts = WtsFile.LoadFromMPQ(mpq);

            //Строки
            WC3Strings strings = Base[gameVer].Clone();
            strings.UpdateFromWts(wts);
            return strings;
        }

        //Клонировать
        public WC3Strings Clone()
        {
            WC3Strings clone = new WC3Strings();
            clone.Dict = new Dictionary<string, string>(Dict);
            return clone;
        }
    }
}
