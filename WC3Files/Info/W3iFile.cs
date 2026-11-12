using WC3Files.Common.MPQ;

namespace WC3Files.Info
{
    public class W3iFile
    {
        /// <summary>
        /// Версия формата
        /// </summary>
        public int FormatVersion
        {
            set; get;
        }

        /// <summary>
        /// Версия карты. Число её сохранений в редакторе.
        /// </summary>
        public int MapVersion
        {
            set; get;
        }

        /// <summary>
        /// Версия редактора.
        /// </summary>
        public int EditorVersion
        {
            set; get;
        }

        /// <summary>
        /// Общая информация, отображаемая в лобби
        /// </summary>
        public W3iLobbyInfo LobbyInfo
        {
            set;
            get;
        }

        /// <summary>
        /// Информация о границе, размерах и ландшафте карты
        /// </summary>
        public W3iMapDimensions MapDimensions
        {
            set;
            get;
        }

        /// <summary>
        /// Загрузочный экран
        /// </summary>
        public W3iLoadingScreen LoadingScreen
        {
            set;
            get;
        }

        /// <summary>
        /// Номер набора игровых данных
        /// </summary>
        public int GameDataSet
        {
            set;
            get;
        }

        /// <summary>
        /// Экран пролога
        /// </summary>
        public W3iPrologueScreen PrologueScreen
        {
            set;
            get;
        }

        /// <summary>
        /// Настройки окружения
        /// </summary>
        public W3iEnvironment Environment
        {
            set;
            get;
        }

        /// <summary>
        /// Слоты игроков
        /// </summary>
        public W3iPlayerSlot[] PlayerSlots
        {
            set;
            get;
        }

        /// <summary>
        /// Команды
        /// </summary>
        public W3iForce[] Forces
        {
            set;
            get;
        }

        /// <summary>
        /// Доступность улучшений
        /// </summary>
        public W3iUpgrade[] Upgrades
        {
            set;
            get;
        }

        /// <summary>
        /// Недоступные технологии
        /// </summary>
        public W3iTech[] Techs
        {
            set;
            get;
        }

        /// <summary>
        /// Группы случайных юнитов
        /// </summary>
        public W3iRandomUnitGroup[] RandomUnitGroups
        {
            set;
            get;
        }

        /// <summary>
        /// Группы случайных предметов
        /// </summary>
        public W3iRandomItemGroup[] RandomItemGroups
        {
            set;
            get;
        }

        //Конструктор
        public W3iFile()
        {

        }

        //Загрузить
        public static W3iFile Load(byte[] data)
        {
            W3iFile f;
            if (data.Length > 0)
            {
                W3iReader r = new W3iReader(data);
                f = r.ReadFile();
            }
            else
            {
                f = new W3iFile();
            }
            return f;
        }
        public static W3iFile LoadFromMPQ(MpqArchive mpq, string path = "war3map.w3i")
        {
            byte[] data;
            using (MpqFile f = mpq.OpenFile(path))
            {
                if (f != null)
                {
                    data = f.Read();
                }
                else
                {
                    data = new byte[0];
                }
            }
            return Load(data);
        }

        //Сохранить
        public void SaveToMPQ(MpqArchive mpq, string path = "war3map.w3i")
        {
            //Данные
            byte[] data = null;
            using (MemoryStream stream = new MemoryStream())
            {
                W3iWriter r = new W3iWriter(stream);
                r.WriteFile(this);
                data = stream.ToArray();
            }

            //Добавить в архив
            mpq.CreateFile(path, data);
        }
    }
}
