using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WC3ProjectManager.Map.Objects
{
    public class WC3ObjectField
    {
        //Id
        public string Id
        {
            set;
            get;
        } = "";

        //Уровень
        public int Level
        {
            set;
            get;
        } = 0;

        //Имя
        public string Name
        {
            set;
            get;
        } = "";

        //Значение
        public object? Value
        {
            set;
            get;
        }

        //Конструктор
        public WC3ObjectField()
        {

        }
        public WC3ObjectField(string name, object? value) : this()
        {
            Name = name;
            Value = value;
        }

        //Клонировать
        public WC3ObjectField Clone()
        {
            return new()
            {
                Id = Id,
                Name = Name,
                Level = Level,
                Value = Value
            };
        }

        //В строку
        public override string ToString()
        {
            return $"[{Id}] {Name} = {Value}";
        }
    }
}
