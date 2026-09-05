namespace WC3ProjectManager
{
    public class TriggerCategory
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
        public List<ITrigger> Triggers
        {
            get;
            private set;
        } = new();
        public ITrigger this[int index] => Triggers[index];
        public ITrigger this[string name] => Triggers.First(x => x.Name == name);

        //В строку
        public override string ToString()
        {
            return Name;
        }
    }
}