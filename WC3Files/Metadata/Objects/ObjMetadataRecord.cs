namespace WC3Files.Metadata.Objects
{
    public class ObjMetadataRecord
    {
        //Id
        public string Id
        {
            get
            {
                return (string)Fields["ID"].Value;
            }
        }

        //Имя
        public string? Name
        {
            get
            {
                string? s = null;
                if (Fields.TryGetValue("field", out ObjMetadataField f))
                {
                    s = (string)f.Value;
                }
                return s;
            }
        }

        //Тип
        public string? Type
        {
            get
            {
                string? s = null;
                if (Fields.TryGetValue("type", out ObjMetadataField f))
                {
                    s = (string)f.Value;
                }
                return s;
            }
        }

        //Название SLK
        public string? SlkName
        {
            get
            {
                string? s = null;
                if (Fields.TryGetValue("slk", out ObjMetadataField? f))
                {
                    s = (string?)f.Value;
                }
                return s;
            }
        }

        //Индекс
        public int Index
        {
            get
            {
                return (int)Fields["index"].Value;
            }
        }

        //Категория
        public string Category
        {
            get
            {
                return (string)Fields["category"].Value;
            }
        }

        //Указатель
        public int Data
        {
            get
            {
                int data = 0;
                if (Fields.TryGetValue("data", out ObjMetadataField? f))
                {
                    data = (int)f.Value;
                }
                return data;
            }
        }

        //Поля
        public Dictionary<string, ObjMetadataField> Fields
        {
            private set;
            get;
        } = new();
        public object? GetFieldValue(string id)
        {
            object? value = null;
            if (Fields.TryGetValue(id, out ObjMetadataField f))
            {
                value = f.Value;
            }
            return value;
        }

        //Конструктор
        public ObjMetadataRecord()
        {

        }

        //В строку
        public override string ToString()
        {
            return Name;
        }
    }
}
