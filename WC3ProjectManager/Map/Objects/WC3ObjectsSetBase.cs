using WC3Files.Metadata;
using WC3Files.Objects;

namespace WC3ProjectManager.Map.Objects
{
    public class WC3ObjectsSetBase
    {
        public WC3ObjectsSet this[string gameVer]
        {
            get
            {
                WC3ObjectsSet objs;
                if (!Dict.TryGetValue(gameVer, out objs))
                {
                    objs = Load(gameVer);
                }
                return objs;
            }
        }
        private Dictionary<string, WC3ObjectsSet> Dict = new();
        public WC3ObjectsSet Load(string gameVer)
        {
            //Сет
            WC3ObjectsSet objs = new WC3ObjectsSet(gameVer);
            Dict[gameVer] = objs;

            //Пути
            string dataDir = $"{AppContext.BaseDirectory}\\GameData\\";
            string versDir = $"{dataDir}{gameVer}\\";

            //Макет
            WC3ObjectLayout.LoadFromIni(dataDir + "ObjectLayout.ini");

            //Юниты
            WC3Objects units = new WC3Objects(W3objTypes.Unit, gameVer);
            objs[W3objTypes.Unit] = units;
            units.UpdateFromW3slk(versDir + "UnitData.slk", true);
            units.UpdateFromW3slk(versDir + "UnitBalance.slk");
            units.UpdateFromW3slk(versDir + "UnitAbilities.slk");
            units.UpdateFromW3slk(versDir + "UnitWeapons.slk");
            units.UpdateFromW3slk(versDir + "UnitUI.slk");
            units.UpdateFromW3ini(versDir + "HumanUnitFunc.txt");
            units.UpdateFromW3ini(versDir + "HumanUnitStrings.txt");
            units.UpdateFromW3ini(versDir + "OrcUnitFunc.txt");
            units.UpdateFromW3ini(versDir + "OrcUnitStrings.txt");
            units.UpdateFromW3ini(versDir + "UndeadUnitFunc.txt");
            units.UpdateFromW3ini(versDir + "UndeadUnitStrings.txt");
            units.UpdateFromW3ini(versDir + "NightElfUnitFunc.txt");
            units.UpdateFromW3ini(versDir + "NightElfUnitStrings.txt");
            units.UpdateFromW3ini(versDir + "NeutralUnitFunc.txt");
            units.UpdateFromW3ini(versDir + "NeutralUnitStrings.txt");
            units.UpdateFromW3ini(versDir + "CampaignUnitFunc.txt");
            units.UpdateFromW3ini(versDir + "CampaignUnitStrings.txt");

            //Предметы
            WC3Objects items = new WC3Objects(W3objTypes.Item, gameVer);
            objs[W3objTypes.Item] = items;
            items.UpdateFromW3slk(versDir + "ItemData.slk", true);
            items.UpdateFromW3ini(versDir + "ItemFunc.txt");
            items.UpdateFromW3ini(versDir + "ItemStrings.txt");

            //Способности
            WC3Objects abils = new WC3Objects(W3objTypes.Ability, gameVer);
            objs[W3objTypes.Ability] = abils;
            abils.UpdateFromW3slk(versDir + "AbilityData.slk", true);
            abils.UpdateFromW3ini(versDir + "HumanAbilityFunc.txt");
            abils.UpdateFromW3ini(versDir + "HumanAbilityStrings.txt");
            abils.UpdateFromW3ini(versDir + "OrcAbilityFunc.txt");
            abils.UpdateFromW3ini(versDir + "OrcAbilityStrings.txt");
            abils.UpdateFromW3ini(versDir + "UndeadAbilityFunc.txt");
            abils.UpdateFromW3ini(versDir + "UndeadAbilityStrings.txt");
            abils.UpdateFromW3ini(versDir + "NightElfAbilityFunc.txt");
            abils.UpdateFromW3ini(versDir + "NightElfAbilityStrings.txt");
            abils.UpdateFromW3ini(versDir + "NeutralAbilityFunc.txt");
            abils.UpdateFromW3ini(versDir + "NeutralAbilityStrings.txt");
            abils.UpdateFromW3ini(versDir + "CommonAbilityFunc.txt");
            abils.UpdateFromW3ini(versDir + "CommonAbilityStrings.txt");
            abils.UpdateFromW3ini(versDir + "CampaignAbilityFunc.txt");
            abils.UpdateFromW3ini(versDir + "CampaignAbilityStrings.txt");
            abils.UpdateFromW3ini(versDir + "ItemAbilityFunc.txt");
            abils.UpdateFromW3ini(versDir + "ItemAbilityStrings.txt");

            //Баффы
            WC3Objects buffs = new WC3Objects(W3objTypes.Buff, gameVer);
            objs[W3objTypes.Buff] = buffs;
            buffs.UpdateFromW3slk(versDir + "AbilityBuffData.slk", true);
            buffs.UpdateFromW3ini(versDir + "HumanAbilityFunc.txt");
            buffs.UpdateFromW3ini(versDir + "HumanAbilityStrings.txt");
            buffs.UpdateFromW3ini(versDir + "OrcAbilityFunc.txt");
            buffs.UpdateFromW3ini(versDir + "OrcAbilityStrings.txt");
            buffs.UpdateFromW3ini(versDir + "UndeadAbilityFunc.txt");
            buffs.UpdateFromW3ini(versDir + "UndeadAbilityStrings.txt");
            buffs.UpdateFromW3ini(versDir + "NightElfAbilityFunc.txt");
            buffs.UpdateFromW3ini(versDir + "NightElfAbilityStrings.txt");
            buffs.UpdateFromW3ini(versDir + "NeutralAbilityFunc.txt");
            buffs.UpdateFromW3ini(versDir + "NeutralAbilityStrings.txt");
            buffs.UpdateFromW3ini(versDir + "CommonAbilityFunc.txt");
            buffs.UpdateFromW3ini(versDir + "CommonAbilityStrings.txt");
            buffs.UpdateFromW3ini(versDir + "CampaignAbilityFunc.txt");
            buffs.UpdateFromW3ini(versDir + "CampaignAbilityStrings.txt");
            buffs.UpdateFromW3ini(versDir + "ItemAbilityFunc.txt");
            buffs.UpdateFromW3ini(versDir + "ItemAbilityStrings.txt");

            //Улучшения
            WC3Objects upgrades = new WC3Objects(W3objTypes.Upgrade, gameVer);
            objs[W3objTypes.Upgrade] = upgrades;
            upgrades.UpdateFromW3slk(versDir + "UpgradeData.slk", true);
            upgrades.UpdateFromW3ini(versDir + "HumanUpgradeFunc.txt");
            upgrades.UpdateFromW3ini(versDir + "HumanUpgradeStrings.txt");
            upgrades.UpdateFromW3ini(versDir + "OrcUpgradeFunc.txt");
            upgrades.UpdateFromW3ini(versDir + "OrcUpgradeStrings.txt");
            upgrades.UpdateFromW3ini(versDir + "UndeadUpgradeFunc.txt");
            upgrades.UpdateFromW3ini(versDir + "UndeadUpgradeStrings.txt");
            upgrades.UpdateFromW3ini(versDir + "NightElfUpgradeFunc.txt");
            upgrades.UpdateFromW3ini(versDir + "NightElfUpgradeStrings.txt");
            upgrades.UpdateFromW3ini(versDir + "NeutralUpgradeFunc.txt");
            upgrades.UpdateFromW3ini(versDir + "NeutralUpgradeStrings.txt");
            upgrades.UpdateFromW3ini(versDir + "CampaignUpgradeFunc.txt");
            upgrades.UpdateFromW3ini(versDir + "CampaignUpgradeStrings.txt");

            //Декорации
            WC3Objects doodads = new WC3Objects(W3objTypes.Doodad, gameVer);
            objs[W3objTypes.Doodad] = doodads;
            doodads.UpdateFromW3slk(versDir + "DoodadData.slk", true);

            //Разрушаемые объекты
            WC3Objects destructs = new WC3Objects(W3objTypes.Destructable, gameVer);
            objs[W3objTypes.Destructable] = destructs;
            destructs.UpdateFromW3slk(versDir + "DestructableData.slk", true);
            return objs;
        }
    }
}
