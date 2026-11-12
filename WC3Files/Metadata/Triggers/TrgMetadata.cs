namespace WC3Files.Metadata.Triggers
{
    public class TrgMetadata
    {
        //Функции
        private Dictionary<string, TrgFunction> FunctionsDict = new();
        internal void AddFunction(TrgFunction f)
        {
            FunctionsDict[f.Name] = f;
        }
        public TrgFunction GetFunction(string name)
        {
            return FunctionsDict[name];
        }

        //Загрузка
        public static TrgMetadata LoadFromFile(string path)
        {
            TrgMetadataReader parser = new TrgMetadataReader(path);
            return parser.ReadData();
        }
    }
}
