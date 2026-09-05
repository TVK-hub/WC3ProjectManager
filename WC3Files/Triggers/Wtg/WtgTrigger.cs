using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WC3Files.Triggers
{
    public class WtgTrigger
    {
        public string Name;
        public string Description;

        public bool IsComment;
        public bool IsEnabled;
        public bool IsCustomText;
        public bool IsInitiallyOff;
        public bool RunOnInit;

        public int CategoryId;

        public List<WtgEca> ECA { set; get; } = new();

        public override string ToString()
        {
            return Name;
        }
    }
}
