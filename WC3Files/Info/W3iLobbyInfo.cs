using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WC3Files.Info
{
    public class W3iLobbyInfo
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Name
        {
            set; get;
        }

        /// <summary>
        /// Автор
        /// </summary>
        public string Author
        {
            set; get;
        }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description
        {
            set; get;
        }

        /// <summary>
        /// Рекомендуемое кол-во игроков
        /// </summary>
        public string PlayersRecommended
        {
            set; get;
        }
    }
}
