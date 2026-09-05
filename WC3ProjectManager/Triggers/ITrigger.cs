using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WC3ProjectManager
{
    public interface ITrigger
    {
        //Название
        string Name
        {
            set; get;
        }

        //Описание
        string Description
        {
            set; get;
        }
    }
}
