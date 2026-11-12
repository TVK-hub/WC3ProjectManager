namespace WC3ProjectManager.Map.Triggers
{
    public class WC3TriggerCategory
    {
        //Id
        public int Id
        {
            set; get;
        }

        //Название
        public string Name
        {
            set; get;
        }

        //Триггеры
        public List<IWC3Trigger> Triggers
        {
            get;
            private set;
        } = new();
        public IWC3Trigger this[int index] => Triggers[index];
        public IWC3Trigger this[string name] => Triggers.First(x => x.Name == name);

        //В строку
        public override string ToString()
        {
            return Name;
        }
    }
}