using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WC3Files.Triggers
{
    public class WtgVariable
    {
        public string Name;
        public string Type;
        public int Unknown;
        public bool IsArray;
        public int ArraySize;
        public bool IsInitialized;
        public string Value;
        public override string ToString()
        {
            return Name;
        }
    }
}
