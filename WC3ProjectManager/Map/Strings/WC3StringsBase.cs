namespace WC3ProjectManager.Map.Strings
{
    public class WC3StringsBase
    {
        public WC3Strings this[string gameVer]
        {
            get
            {
                WC3Strings strings;
                if (!Dict.TryGetValue(gameVer, out strings))
                {
                    strings = Load(gameVer);
                }
                return strings;
            }
        }
        private Dictionary<string, WC3Strings> Dict = new();
        public WC3Strings Load(string gameVer)
        {
            //Сет
            WC3Strings strings = new WC3Strings();
            Dict[gameVer] = strings;

            //Путь
            string versDir = $"{AppContext.BaseDirectory}\\GameData\\{gameVer}\\";

            //Загрузка
            strings.UpdateFromIni(versDir + "WorldEditStrings.txt");
            strings.UpdateFromIni(versDir + "WorldEditGameStrings.txt");
            return strings;
        }
    }
}