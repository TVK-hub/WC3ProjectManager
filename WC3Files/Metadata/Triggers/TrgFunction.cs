using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WC3Files.Metadata.Triggers
{
    public class TrgFunction
    {
        //Название
        public string Name
        {
            set; get;
        }

        //Параметры
        public List<string> Parameters
        {
            get;
        } = new();

        //Значение на выход
        public string? ReturnValue
        {
            set; get;
        } = null;

        //Конструктор
        public TrgFunction(string name)
        {
            Name = name;
        }
    }
}
