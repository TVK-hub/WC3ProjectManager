namespace WC3ProjectManager.Map.Objects
{
    public class WC3ObjectFieldLayout
    {
        //Id
        public string Id
        {
            set;
            get;
        }

        //Имя
        public string Name
        {
            set;
            get;
        } = "";

        //Индекс
        //По нему происходит сортировка полей
        public int Index
        {
            set;
            get;
        }

        //Секция
        public string SectionName
        {
            set;
            get;
        }
        public int SectionIndex
        {
            set;
            get;
        }
    }
}
