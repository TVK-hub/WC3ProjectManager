using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WC3Files.Triggers
{
    public class WtgEca
    {
        public WtgEcaType Type
        {
            get;
            set;
        }
        public WtgEcaGroup Group
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        } = "";
        public bool IsEnabled
        {
            get;
            set;
        }
        public List<WtgParameter> Parameters
        {
            get;
        } = new();
        public List<WtgEca> Childrens
        {
            get;
        } = new();
        public override string ToString()
        {
            return Name;
        }
    }
}
