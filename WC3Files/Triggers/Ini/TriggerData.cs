using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WC3Files.Triggers
{
    public class TriggerData
    {
        //Функции
        private Dictionary<string, TriggerFunction> FunctionsDict = new();
        internal void AddFunction(TriggerFunction f)
        {
            FunctionsDict[f.Name] = f;
        }
        public TriggerFunction GetFunction(string name)
        {
            return FunctionsDict[name];
        }

        //Парсинг
        public static TriggerData Load(string path)
        {
            TriggerDataParser parser = new TriggerDataParser();
            return parser.Parse(path);
        }
    }
}
