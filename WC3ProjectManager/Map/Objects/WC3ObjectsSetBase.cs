using WC3Files.Metadata;
using WC3Files.Objects;
using WC3ProjectManager.Map.Strings;

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
            //Строки
            WC3Strings strings = WC3Strings.Base[gameVer];

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
            units.UpdateFromW3slk(versDir + "UnitData.slk", strings, true);
            units.UpdateFromW3slk(versDir + "UnitBalance.slk", strings);
            units.UpdateFromW3slk(versDir + "UnitAbilities.slk", strings);
            units.UpdateFromW3slk(versDir + "UnitWeapons.slk", strings);
            units.UpdateFromW3slk(versDir + "UnitUI.slk", strings);
            units.UpdateFromW3ini(versDir + "HumanUnitFunc.txt", strings);
            units.UpdateFromW3ini(versDir + "HumanUnitStrings.txt", strings);
            units.UpdateFromW3ini(versDir + "OrcUnitFunc.txt", strings);
            units.UpdateFromW3ini(versDir + "OrcUnitStrings.txt", strings);
            units.UpdateFromW3ini(versDir + "UndeadUnitFunc.txt", strings);
            units.UpdateFromW3ini(versDir + "UndeadUnitStrings.txt", strings);
            units.UpdateFromW3ini(versDir + "NightElfUnitFunc.txt", strings);
            units.UpdateFromW3ini(versDir + "NightElfUnitStrings.txt", strings);
            units.UpdateFromW3ini(versDir + "NeutralUnitFunc.txt", strings);
            units.UpdateFromW3ini(versDir + "NeutralUnitStrings.txt", strings);
            units.UpdateFromW3ini(versDir + "CampaignUnitFunc.txt", strings);
            units.UpdateFromW3ini(versDir + "CampaignUnitStrings.txt", strings);

            //Предметы
            WC3Objects items = new WC3Objects(W3objTypes.Item, gameVer);
            objs[W3objTypes.Item] = items;
            items.UpdateFromW3slk(versDir + "ItemData.slk", strings, true);
            items.UpdateFromW3ini(versDir + "ItemFunc.txt", strings);
            items.UpdateFromW3ini(versDir + "ItemStrings.txt", strings);

            //Способности
            WC3Objects abils = new WC3Objects(W3objTypes.Ability, gameVer);
            objs[W3objTypes.Ability] = abils;
            abils.UpdateFromW3slk(versDir + "AbilityData.slk", strings, true);
            abils.UpdateFromW3ini(versDir + "HumanAbilityFunc.txt", strings);
            abils.UpdateFromW3ini(versDir + "HumanAbilityStrings.txt", strings);
            abils.UpdateFromW3ini(versDir + "OrcAbilityFunc.txt", strings);
            abils.UpdateFromW3ini(versDir + "OrcAbilityStrings.txt", strings);
            abils.UpdateFromW3ini(versDir + "UndeadAbilityFunc.txt", strings);
            abils.UpdateFromW3ini(versDir + "UndeadAbilityStrings.txt", strings);
            abils.UpdateFromW3ini(versDir + "NightElfAbilityFunc.txt", strings);
            abils.UpdateFromW3ini(versDir + "NightElfAbilityStrings.txt", strings);
            abils.UpdateFromW3ini(versDir + "NeutralAbilityFunc.txt", strings);
            abils.UpdateFromW3ini(versDir + "NeutralAbilityStrings.txt", strings);
            abils.UpdateFromW3ini(versDir + "CommonAbilityFunc.txt", strings);
            abils.UpdateFromW3ini(versDir + "CommonAbilityStrings.txt", strings);
            abils.UpdateFromW3ini(versDir + "CampaignAbilityFunc.txt", strings);
            abils.UpdateFromW3ini(versDir + "CampaignAbilityStrings.txt", strings);
            abils.UpdateFromW3ini(versDir + "ItemAbilityFunc.txt", strings);
            abils.UpdateFromW3ini(versDir + "ItemAbilityStrings.txt", strings);

            //Баффы
            WC3Objects buffs = new WC3Objects(W3objTypes.Buff, gameVer);
            objs[W3objTypes.Buff] = buffs;
            buffs.UpdateFromW3slk(versDir + "AbilityBuffData.slk", strings, true);
            buffs.UpdateFromW3ini(versDir + "HumanAbilityFunc.txt", strings);
            buffs.UpdateFromW3ini(versDir + "HumanAbilityStrings.txt", strings);
            buffs.UpdateFromW3ini(versDir + "OrcAbilityFunc.txt", strings);
            buffs.UpdateFromW3ini(versDir + "OrcAbilityStrings.txt", strings);
            buffs.UpdateFromW3ini(versDir + "UndeadAbilityFunc.txt", strings);
            buffs.UpdateFromW3ini(versDir + "UndeadAbilityStrings.txt", strings);
            buffs.UpdateFromW3ini(versDir + "NightElfAbilityFunc.txt", strings);
            buffs.UpdateFromW3ini(versDir + "NightElfAbilityStrings.txt", strings);
            buffs.UpdateFromW3ini(versDir + "NeutralAbilityFunc.txt", strings);
            buffs.UpdateFromW3ini(versDir + "NeutralAbilityStrings.txt", strings);
            buffs.UpdateFromW3ini(versDir + "CommonAbilityFunc.txt", strings);
            buffs.UpdateFromW3ini(versDir + "CommonAbilityStrings.txt", strings);
            buffs.UpdateFromW3ini(versDir + "CampaignAbilityFunc.txt", strings);
            buffs.UpdateFromW3ini(versDir + "CampaignAbilityStrings.txt", strings);
            buffs.UpdateFromW3ini(versDir + "ItemAbilityFunc.txt", strings);
            buffs.UpdateFromW3ini(versDir + "ItemAbilityStrings.txt", strings);

            //Улучшения
            WC3Objects upgrades = new WC3Objects(W3objTypes.Upgrade, gameVer);
            objs[W3objTypes.Upgrade] = upgrades;
            upgrades.UpdateFromW3slk(versDir + "UpgradeData.slk", strings, true);
            upgrades.UpdateFromW3ini(versDir + "HumanUpgradeFunc.txt", strings);
            upgrades.UpdateFromW3ini(versDir + "HumanUpgradeStrings.txt", strings);
            upgrades.UpdateFromW3ini(versDir + "OrcUpgradeFunc.txt", strings);
            upgrades.UpdateFromW3ini(versDir + "OrcUpgradeStrings.txt", strings);
            upgrades.UpdateFromW3ini(versDir + "UndeadUpgradeFunc.txt", strings);
            upgrades.UpdateFromW3ini(versDir + "UndeadUpgradeStrings.txt", strings);
            upgrades.UpdateFromW3ini(versDir + "NightElfUpgradeFunc.txt", strings);
            upgrades.UpdateFromW3ini(versDir + "NightElfUpgradeStrings.txt", strings);
            upgrades.UpdateFromW3ini(versDir + "NeutralUpgradeFunc.txt", strings);
            upgrades.UpdateFromW3ini(versDir + "NeutralUpgradeStrings.txt", strings);
            upgrades.UpdateFromW3ini(versDir + "CampaignUpgradeFunc.txt", strings);
            upgrades.UpdateFromW3ini(versDir + "CampaignUpgradeStrings.txt", strings);

            //Декорации
            WC3Objects doodads = new WC3Objects(W3objTypes.Doodad, gameVer);
            objs[W3objTypes.Doodad] = doodads;
            doodads.UpdateFromW3slk(versDir + "DoodadData.slk", strings, true);

            //Разрушаемые объекты
            WC3Objects destructs = new WC3Objects(W3objTypes.Destructable, gameVer);
            objs[W3objTypes.Destructable] = destructs;
            destructs.UpdateFromW3slk(versDir + "DestructableData.slk", strings, true);
            return objs;
        }
    }
}
