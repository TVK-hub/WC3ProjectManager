using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WC3Files.Ini
{
    public class IniEntry
    {
        public string Name  = "";
        public string Value = "";
        public IniSection Section;
        public IniEntry(IniSection s)
        {
            Section = s;
        }
    }
}
