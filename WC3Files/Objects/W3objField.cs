namespace WC3Files.Objects
{
    public class W3objField
    {
        //Id
        public string Id
        {
            get;
            set;
        } = "";

        //Тип значения
        public W3objValueType ValueType
        {
            get;
            set;
        }

        //Уровень или Вариация
        public int Level
        {
            get;
            set;
        }

        /// <summary>
        ///Индекс данных.
        ///Используется для выбора варианта поля; например, для некоторых полей значения 1, 2, 3 соответствуют суффиксам A, B, C.
        /// </summary>
        public int DataPointer
        {
            get;
            set;
        }

        //Значение
        public object? Value;
    }
}
