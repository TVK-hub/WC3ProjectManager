using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WC3ProjectManager
{
    public class WC3ProjectDirectories
    {
        //Корень
        public string Root { get; }

        //Директории
        public string Build => Path.Combine(Root, "Build");
        public string Maps => Path.Combine(Root, "Maps");
        public string Triggers => Path.Combine(Root, "Triggers");
        public string Import => Path.Combine(Root, "Import");

        //Конструктор
        public WC3ProjectDirectories(string root)
        {
            Root = root;
        }
    }
}
