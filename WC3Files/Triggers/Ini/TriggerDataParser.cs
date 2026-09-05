using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace WC3Files.Triggers
{
    public class TriggerDataParser
    {
        //Конструктор
        public TriggerDataParser()
        {
        
        }

        //Парсинг
        public TriggerData Parse(string path)
        {
            //Данные
            TriggerData data = new TriggerData();

            //Файл
            string? section = null;
            foreach (string line in File.ReadLines(path))
            {
                //Строка
                string trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;

                //Секция
                if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                {
                    section = trimmed[1..^1];
                    continue;
                }

                //Функция
                if (
                    section == "TriggerActions"    ||
                    section == "TriggerEvents"     ||
                    section == "TriggerConditions" ||
                    section == "TriggerCalls"
                )
                {
                    if (!trimmed.StartsWith("//") && !trimmed.StartsWith("_")) //Пропуск комментариев и служебных ключей
                    {
                        TriggerFunction f = ReadFunction(section, line);
                        data.AddFunction(f);
                    }
                }
            }
            return data;
        }
        public TriggerFunction ReadFunction(string section, string line)
        {
            //Название и значение
            int separator = line.IndexOf('=');
            string name = line[..separator];
            string value = line[(separator + 1)..];
            
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
                    if (f.ReturnValue==null && section == "TriggerCalls") { f.ReturnValue = p; }
                    else { f.Parameters.Add(p); }
                }
            }
            return f;
        }
    }
}
