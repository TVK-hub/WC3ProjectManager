using WC3Files.Metadata.Objects;
using WC3Files.Metadata.Triggers;
using WC3Files.Objects;
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
        public Dictionary<W3objTypes, ObjMetadata> Objects
        {
            get;
            private set;
        }

        //Загрузка
        public void Load(string dir)
        {
            Triggers = TrgMetadata.LoadFromFile(dir+"TriggerData.txt");
            Objects = new Dictionary<W3objTypes, ObjMetadata>();
            Objects[W3objTypes.Unit] = ObjMetadata.LoadFromFile(dir+"UnitMetaData.slk");
            Objects[W3objTypes.Item] = ObjMetadata.LoadFromFile(dir+"UnitMetaData.slk");
            Objects[W3objTypes.Ability] = ObjMetadata.LoadFromFile(dir + "AbilityMetaData.slk");
            Objects[W3objTypes.Buff] = ObjMetadata.LoadFromFile(dir + "AbilityBuffMetaData.slk");
            Objects[W3objTypes.Upgrade] = ObjMetadata.LoadFromFile(dir + "UpgradeMetaData.slk");
            Objects[W3objTypes.Doodad] = ObjMetadata.LoadFromFile(dir + "DoodadMetaData.slk");
            Objects[W3objTypes.Destructable] = ObjMetadata.LoadFromFile(dir + "DestructableMetaData.slk");
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
