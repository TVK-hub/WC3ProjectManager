namespace WC3Files.Objects
{
    public class W3objMod
    {
        //Id
        public string BaseId
        {
            get;
            set;
        } = "\0\0\0\0";
        public string CustomId
        {
            get;
            set;
        } = "\0\0\0\0";

        //Поля
        public List<W3objField> Fields
        {
            get;
            private set;
        }

        //Конструктор
        public W3objMod()
        {
            Fields = new List<W3objField>();
        }
    }
}
