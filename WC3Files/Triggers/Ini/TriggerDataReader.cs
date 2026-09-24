using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WC3Files.Ini;
using static System.Collections.Specialized.BitVector32;

namespace WC3Files.Triggers
{
    public class TriggerDataReader
    {
        //Файл
        IniFile Ini;

        //Конструктор
        public TriggerDataReader(string path)
        {
            Ini = IniFile.Load(path);
        }

        //Парсинг
        public TriggerData ReadData()
        {
            //Данные
            TriggerData data = new TriggerData();

            //Разбор
            foreach (IniEntry en in Ini.Entries)
            {
                if (!en.Name.StartsWith("_")) //Пропуск служебных ключей
                switch (en.Section.Name)
                {
                    case "TriggerActions":
                    case "TriggerEvents":
                    case "TriggerConditions":
                    case "TriggerCalls":
                        TriggerFunction f = ReadFunction(en);
                        data.AddFunction(f);
                        break;
                }
            }
            return data;
        }
        public TriggerFunction ReadFunction(IniEntry en)
        {
            //Название и значение
            string name = en.Name;
            string value = en.Value;
            
            //Функция
            TriggerFunction f = new TriggerFunction(name);

            //Параметры
            string[] prms = value.Split(',');
            for (int i = 0; i < prms.Length; i++)
            {
                string p = prms[i];
                bool isNumeric = int.TryParse(p, out _);
                if (!isNumeric && p != "nothing")
                {
                    if (f.ReturnValue==null && en.Section.Name == "TriggerCalls") { f.ReturnValue = p; }
                    else { f.Parameters.Add(p); }
                }
            }
            return f;
        }
    }
}
