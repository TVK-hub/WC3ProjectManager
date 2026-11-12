namespace WC3Files.Common.Ini
{
    public class IniSection
    {
        //Имя
        public string Name;

        //Список
        public List<IniEntry> Entries
        {
            get;
        } = new List<IniEntry>();

        //Конструктор
        public IniSection(string name)
        {
            Name = name;
        }
    }
}
