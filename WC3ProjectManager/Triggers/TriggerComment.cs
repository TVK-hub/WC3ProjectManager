namespace WC3ProjectManager
{
    public class TriggerComment : ITrigger
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