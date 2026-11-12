using WC3Files.Metadata.Objects;
using WC3Files.Metadata.Triggers;
namespace WC3Files.Metadata
{
    public class WC3Metadata
    {
        //Поля
        public TrgMetadata Triggers
        {
            get;
            private set;
        }
        public ObjMetadata Units
        {
            get;
            private set;
        }
        public ObjMetadata Abilities
        {
            get;
            private set;
        }
        public ObjMetadata Buffs
        {
            get;
            private set;
        }
        public ObjMetadata Upgrades
        {
            get;
            private set;
        }
        public ObjMetadata Doodads
        {
            get;
            private set;
        }
        public ObjMetadata Destructables
        {
            get;
            private set;
        }

        //Загрузка
        public void Load(string dir)
        {
            Triggers = TrgMetadata.LoadFromFile(dir+"TriggerData.txt");
            Units = ObjMetadata.LoadFromFile(dir+"UnitMetaData.slk");
            Abilities = ObjMetadata.LoadFromFile(dir+"AbilityMetaData.slk");
            Buffs = ObjMetadata.LoadFromFile(dir+"AbilityBuffMetaData.slk");
            Upgrades = ObjMetadata.LoadFromFile(dir+"UpgradeMetaData.slk");
            Doodads = ObjMetadata.LoadFromFile(dir+"DoodadMetaData.slk");
            Destructables = ObjMetadata.LoadFromFile(dir+"DestructableMetaData.slk");
        }

        //Получить по версии
        public static WC3Metadata GetByVers(string gameVer)
        {
            WC3Metadata meta;
            if (!_Dict.TryGetValue(gameVer, out meta))
            {
                meta = new(); _Dict[gameVer] = meta;
                string versDir = $"{AppContext.BaseDirectory}\\GameData\\{gameVer}\\";
                meta.Load(versDir);
            }
            return meta;
        }
        private static Dictionary<string, WC3Metadata> _Dict = new();
    }
}
