using WC3Files.Common.Ini;
namespace WC3Files.Metadata.Triggers
{
    public class TrgMetadataReader
    {
        //Файл
        IniFile Ini;

        //Конструктор
        public TrgMetadataReader(string path)
        {
            Ini = IniFile.Load(path);
        }

        //Парсинг
        public TrgMetadata ReadData()
        {
            //Данные
            TrgMetadata data = new TrgMetadata();

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
                        TrgFunction f = ReadFunction(en);
                        data.AddFunction(f);
                        break;
                }
            }
            return data;
        }
        public TrgFunction ReadFunction(IniEntry en)
        {
            //Название и значение
            string name = en.Name;
            string value = en.Value;
            
            //Функция
            TrgFunction f = new TrgFunction(name);

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
