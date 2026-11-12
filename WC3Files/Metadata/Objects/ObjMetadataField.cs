namespace WC3Files.Metadata.Objects
{
    public class ObjMetadataField
    {
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
        public ObjMetadataField(string name, object? value)
        {
            Name = name;
            Value = value;
        }
    }
}
