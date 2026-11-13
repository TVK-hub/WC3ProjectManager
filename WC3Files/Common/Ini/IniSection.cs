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

        //Клонировать
        public IniSection Clone()
        {
            IniSection clone = new(Name);
            foreach (IniEntry en in this.Entries)
            {
                IniEntry enClone = en.Clone(clone);
                clone.Entries.Add(enClone);
            }
            return clone;
        }
    }
}
