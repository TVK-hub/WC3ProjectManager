using System.Drawing;
using System.Xml.Linq;

namespace WC3Files.Info
{
    public class W3iReader
    {
        //Ридер
        WC3Reader Reader;

        //Конструктор
        public W3iReader(byte[] data)
        {
            Reader = new WC3Reader(data);
        }

        //Парсинг
        public W3iFile ReadFile()
        {
            //Файл
            var wct = new W3iFile();

            //Версия
            wct.FormatVersion = Reader.ReadInt32();
            wct.MapVersion = Reader.ReadInt32();
            wct.EditorVersion = Reader.ReadInt32();

            //Информация
            wct.LobbyInfo = ReadLobbyInfo();
            wct.MapDimensions = ReadDimensions();
            wct.LoadingScreen = ReadLoadingScreen();
            wct.GameDataSet = Reader.ReadInt32();
            wct.PrologueScreen = ReadPrologueScreen();
            wct.Environment = ReadEnvironment();
            wct.PlayerSlots = ReadPlayerSlots();
            wct.Forces = ReadForces();
            wct.Upgrades = ReadUpgrades();
            wct.Techs = ReadTechs();
            wct.RandomUnitGroups = ReadRandomUnitGroup();
            wct.RandomItemGroups = ReadRandomItemGroup();

            //Вернуть
            return wct;
        }
        public W3iLobbyInfo ReadLobbyInfo()
        {
            W3iLobbyInfo lobby = new W3iLobbyInfo();
            lobby.Name = Reader.ReadCString();
            lobby.Author = Reader.ReadCString();
            lobby.Description = Reader.ReadCString();
            lobby.PlayersRecommended = Reader.ReadCString();
            return lobby;
        }
        public W3iMapDimensions ReadDimensions()
        {
            W3iMapDimensions dim = new W3iMapDimensions();
            for (int i = 0; i < dim.CameraBounds.Length; i++)
            {
                PointF p = new PointF();
                p.X = Reader.ReadSingle();
                p.Y = Reader.ReadSingle();
                dim.CameraBounds[i] = p;
            }
            for (int i = 0; i < dim.CameraBoundsComplements.Length; i++)
            {
                dim.CameraBoundsComplements[i] = Reader.ReadInt32();
            }
            dim.PlayableWidth = Reader.ReadInt32();
            dim.PlayableHeight = Reader.ReadInt32();
            dim.Flags = Reader.ReadInt32();
            dim.Tileset = Reader.ReadChar();
            return dim;
        }
        public W3iLoadingScreen ReadLoadingScreen()
        {
            W3iLoadingScreen sc = new W3iLoadingScreen();
            sc.Number = Reader.ReadInt32();
            sc.Model = Reader.ReadCString();
            sc.Text = Reader.ReadCString();
            sc.Title = Reader.ReadCString();
            sc.Subtitle = Reader.ReadCString();
            return sc;
        }
        public W3iPrologueScreen ReadPrologueScreen()
        {
            W3iPrologueScreen sc = new W3iPrologueScreen();
            sc.Model = Reader.ReadCString();
            sc.Text = Reader.ReadCString();
            sc.Title = Reader.ReadCString();
            sc.Subtitle = Reader.ReadCString();
            return sc;
        }
        public W3iEnvironment ReadEnvironment()
        {
            W3iEnvironment en = new W3iEnvironment();
            en.FogStyle = Reader.ReadInt32();

            en.FogStartZ = Reader.ReadSingle();
            en.FogEndZ = Reader.ReadSingle();
            en.FogDensity = Reader.ReadSingle();

            en.FogColorR = Reader.ReadByte();
            en.FogColorG = Reader.ReadByte();
            en.FogColorB = Reader.ReadByte();
            en.FogColorA = Reader.ReadByte();

            en.WeatherId = Reader.ReadInt32();
            en.SoundEnvironment = Reader.ReadCString();
            en.LightTilesetId = Reader.ReadChar();

            en.WaterColorR = Reader.ReadByte();
            en.WaterColorG = Reader.ReadByte();
            en.WaterColorB = Reader.ReadByte();
            en.WaterColorA = Reader.ReadByte();
            return en;
        }
        public W3iPlayerSlot[] ReadPlayerSlots()
        {
            int len = Reader.ReadInt32();
            W3iPlayerSlot[] slots = new W3iPlayerSlot[len];
            for (int i = 0; i < len; i++)
            {
                W3iPlayerSlot slot = new W3iPlayerSlot();
                slot.Number = Reader.ReadInt32();
                slot.Type = Reader.ReadInt32();
                slot.Race = Reader.ReadInt32();
                slot.FixedStartPosition = Reader.ReadInt32();
                slot.Name = Reader.ReadCString();
                slot.StartX = Reader.ReadSingle();
                slot.StartY = Reader.ReadSingle();
                slot.AllyLowPriorities = Reader.ReadInt32();
                slot.AllyHighPriorities = Reader.ReadInt32();
                slots[i] = slot;
            }
            return slots;
        }
        public W3iForce[] ReadForces()
        {
            int len = Reader.ReadInt32();
            W3iForce[] forces = new W3iForce[len];
            for (int i = 0; i < len; i++)
            {
                W3iForce f = new W3iForce();
                f.Flags = Reader.ReadInt32();
                f.PlayerMasks = Reader.ReadInt32();
                f.Name = Reader.ReadCString();
                forces[i] = f;
            }
            return forces;
        }
        public W3iUpgrade[] ReadUpgrades()
        {
            int len = Reader.ReadInt32();
            W3iUpgrade[] upgrades = new W3iUpgrade[len];
            for (int i = 0; i < len; i++)
            {
                W3iUpgrade upg = new W3iUpgrade();
                upg.PlayerFlags = Reader.ReadInt32();
                upg.Id = Reader.ReadFxString(4);
                upg.Level = Reader.ReadInt32();
                upg.Availability = Reader.ReadInt32();
                upgrades[i] = upg;
            }
            return upgrades;
        }
        public W3iTech[] ReadTechs()
        {
            int len = Reader.ReadInt32();
            W3iTech[] upgrades = new W3iTech[len];
            for (int i = 0; i < len; i++)
            {
                W3iTech upg = new W3iTech();
                upg.PlayerFlags = Reader.ReadInt32();
                upg.Id = Reader.ReadFxString(4);
                upgrades[i] = upg;
            }
            return upgrades;
        }
        public W3iRandomUnitGroup[] ReadRandomUnitGroup()
        {
            int len = Reader.ReadInt32();
            W3iRandomUnitGroup[] grps = new W3iRandomUnitGroup[len];
            for (int i = 0; i < len; i++)
            {
                //Группа
                W3iRandomUnitGroup grp = new W3iRandomUnitGroup();
                grp.Number = Reader.ReadInt32();
                grp.Name = Reader.ReadCString();
                grps[i] = grp;

                //Позиции
                grp.Positions = new int[Reader.ReadInt32()];
                for (int j = 0; j < grp.Positions.Length; j++)
                {
                    grp.Positions[j] = Reader.ReadInt32();
                }

                //Предметы
                grp.Items = new W3iRandomUnitGroupRecord[Reader.ReadInt32()];
                for (int j = 0; j < grp.Items.Length; j++)
                {
                    W3iRandomUnitGroupRecord r = new();
                    r.Chance = Reader.ReadInt32();
                    r.Ids = new string[Reader.ReadInt32()];
                    for (int k = 0; k < r.Ids.Length; k++)
                    {
                        r.Ids[k] = Reader.ReadFxString(4);
                    }
                    grp.Items[j] = r;
                }
            }
            return grps;
        }
        public W3iRandomItemGroup[] ReadRandomItemGroup()
        {
            int len = Reader.ReadInt32();
            W3iRandomItemGroup[] grps = new W3iRandomItemGroup[len];
            for (int i = 0; i < len; i++)
            {
                //Группа
                W3iRandomItemGroup grp = new W3iRandomItemGroup();
                grp.Number = Reader.ReadInt32();
                grp.Name = Reader.ReadCString();
                grps[i] = grp;

                //Предметы
                grp.Items = new W3iRandomItem[Reader.ReadInt32()];
                for (int j = 0; j < grp.Items.Length; j++)
                {
                    W3iRandomItem itm = new();
                    itm.Chance = Reader.ReadInt32();
                    itm.Id = Reader.ReadFxString(4);
                    grp.Items[j] = itm;
                }
            }
            return grps;
        }
    }
}
