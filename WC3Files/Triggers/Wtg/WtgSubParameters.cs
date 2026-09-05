using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WC3Files.Triggers
{
    public class WtgSubParameters
    {
        public WtgParameterTypes Type
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        } = "";
        public WtgParameter[]? Parameters
        {
            get;
            set;
        } = null;
    }
}
