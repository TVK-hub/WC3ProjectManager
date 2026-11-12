namespace WC3Files.Common.SLK
{
    public class SlkFile
    {
        //Ячейки
        public object? this[int row, int column]
        {
            set
            {
                _Cells[(row, column)] = value;
            }
            get
            {
                _Cells.TryGetValue((row, column), out object? value);
                return value;
            }
        }
        private Dictionary<(int Row, int Column), object> _Cells = new();

        //Кол-во
        public int RowsCount
        {
            get;
            set;
        }
        public int ColumnsCount
        {
            get;
            set;
        }

        //Конструктор
        public SlkFile()
        {

        }

        //Загрузить
        public static SlkFile Load(string path)
        {
            SlkReader r = new SlkReader(path);
            return r.ReadFile();
        }
    }
}
