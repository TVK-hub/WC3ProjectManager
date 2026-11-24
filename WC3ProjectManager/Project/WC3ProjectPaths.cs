namespace WC3ProjectManager.Project
{
    public class WC3ProjectPaths
    {
        //Директории
        public string Build
        {
            get;
            set;
        } = "Build";
        public string Maps
        {
            get;
            set;
        } = "Maps";
        public string Config
        {
            get;
            set;
        } = "Config";
        public string[] Objects
        {
            get;
            set;
        } = { "Objects" };
        public string[] Triggers
        {
            get;
            set;
        } = { "Triggers" };
        public string Import
        {
            get;
            set;
        } = "Import";

        //Инициализация
        public void Init(string dir)
        {
            Build = Path.Combine(dir, Build);
            Maps = Path.Combine(dir, Maps);
            Config = Path.Combine(dir, Config);
            Import = Path.Combine(dir, Import);
            for (int i = 0; i < Objects.Length; i++)
            {
                Objects[i] = Path.Combine(dir, Objects[i]);
            }
            for (int i = 0; i < Triggers.Length; i++)
            {
                Triggers[i] = Path.Combine(dir, Triggers[i]);
            }
        }
    }
}