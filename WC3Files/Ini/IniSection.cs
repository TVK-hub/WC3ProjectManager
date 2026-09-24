using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WC3Files.Ini
{
    public class IniSection
    {
        //Имя
        public string Name;

        //Список
        public List<IniEntry> Entries
        {
            get;
        } = new List<IniEntry>();

        //Конструктор
        public IniSection(string name)
        {
            Name = name;
        }
    }
}
