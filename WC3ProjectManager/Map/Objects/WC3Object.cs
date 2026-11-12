using WC3Files.Objects;

namespace WC3ProjectManager.Map.Objects
{
    public class WC3Object
    {
        //Id
        public string Id
        {
            get
            {
                return (string)Fields.GetByName("ID").Value;
            }
            set
            {
                var f = Fields.GetByName("ID");
                f.Value = value;
            }
        }

        //Базовый Id
        public string BaseId
        {
            get
            {
                string id = "";
                var f = Fields.GetByName("ID_Base");
                if (f!=null) { id = f.Value.ToString(); }
                return id;
            }
            set
            {
                var f = Fields.GetByName("ID_Base");
                f.Value = value;
            }
        }

        //Тип объекта
        public W3objTypes WC3Type;

        //Имя
        public string Name
        {
            get
            {
                //Поле
                WC3ObjectField? f = Fields.GetByName("Name");
                if (f==null) { f = Fields.GetByName("EditorName"); }
                if (f==null) { f = Fields.GetByName("Bufftip"); }

                //Значение
                string s;
                if (f != null)
                {
                    s = f.Value?.ToString();
                }
                else
                {
                    s = "";
                }
                return s;
            }
            set
            {
                Fields["Name", 0].Value = value;
            }
        }

        //Суффикс
        public string EditorSuffix
        {
            get
            {
                //Поле
                WC3ObjectField? f = Fields.GetByName("EditorSuffix");

                //Значение
                string s;
                if (f != null)
                {
                    s = f.Value?.ToString();
                }
                else
                {
                    s = "";
                }
                return s;
            }
            set
            {
                Fields["EditorSuffix", 0].Value = value;
            }
        }

        //Поля
        public WC3ObjectFields Fields
        {
            get;
            private set;
        } = new();

        //Конструктор
        public WC3Object(W3objTypes t)
        {
            WC3Type = t;
        }

        //Клонировать
        public WC3Object Clone()
        {
            WC3Object clone = new(WC3Type);
            foreach (var f in Fields.Values)
            {
                clone.Fields[f.Id, f.Level] = f.Clone();
            }
            return clone;
        }

        //В строку
        public override string ToString()
        {
            return Name;
        }
    }
}