namespace WC3ProjectManager.Map.Triggers
{
    public class WC3TriggerComment : IWC3Trigger
    {
        //Название
        public string Name
        {
            set; get;
        } = "";

        //Описание
        public string Description
        {
            set; get;
        } = "";

        //В строку
        public override string ToString()
        {
            return Name;
        }
    }
}