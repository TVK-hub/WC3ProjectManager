using WC3Files.Common.SLK;

namespace WC3Files.Metadata.Objects
{
    public class ObjMetadata
    {
        //Записи
        public Dictionary<string, ObjMetadataRecord> Records
        {
            private set;
            get;
        } = new();

        //Список SLK
        public Dictionary<string, ObjMetadataRecords> SlkRecords
        {
            private set;
            get;
        } = new();

        //Загрузка
        public static ObjMetadata LoadFromFile(string path)
        {
            //Slk
            SlkFile slk = SlkFile.Load(path);

            //Метаданные
            ObjMetadata meta = new();
            for (int r = 1; r < slk.RowsCount; r++)
            {
                //Запись
                ObjMetadataRecord rec = new();

                //Свойства
                for (int c = 0; c < slk.ColumnsCount; c++)
                {
                    string? name = (string?)slk[0, c];
                    object? value = slk[r, c];
                    if (name != null)
                    {
                        rec.Fields[name] = new ObjMetadataField(name, value);
                    }
                }

                //Добавить в коллекции
                ObjMetadataRecords lst;
                if (!meta.SlkRecords.TryGetValue(rec.SlkName, out lst))
                {
                    lst = new ObjMetadataRecords();
                    meta.SlkRecords[rec.SlkName] = lst;
                }
                lst.Add(rec);
                meta.Records[rec.Id] = rec;
            }
            return meta;
        }
    }
}
