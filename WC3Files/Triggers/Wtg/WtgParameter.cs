using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WC3Files.Triggers
{
    public class WtgParameter
    {
        public WtgParameterTypes Type
        {
            get;
            set;
        }
        public string Value
        {
            get;
            set;
        } = "";
        public WtgSubParameters? SubParameters
        {
            get;
            set;
        } = null;
        public bool IsArray
        {
            set; get;
        }
        public WtgParameter ArrayIndex
        {
            get;
            set;
        }
    }
}
