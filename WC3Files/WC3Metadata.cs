using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WC3Files.Triggers;

namespace WC3Files
{
    public static class WC3Metadata
    {
        //Поля
        public static TriggerData Trigger
        {
            get;
            private set;
        }

        //Загрузка
        public static void Load()
        {
            Trigger = TriggerData.Load(AppContext.BaseDirectory + "TriggerData.txt");
        }
    }
}
