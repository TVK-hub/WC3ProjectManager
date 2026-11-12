using WC3Files.Objects;

namespace WC3Files.Metadata.Objects
{
    public class ObjMetadataRecords
    {
        //Список
        List<ObjMetadataRecord> _List = new();
        public ObjMetadataRecord this[int inx]
        {
            get
            {
                return _List[inx];
            }
            //set
            //{
            //    _List[inx] = value;
            //}
        }

        //По имени
        private Dictionary<string, List<ObjMetadataRecord>> _DictByName = new();
        public ObjMetadataRecord? GetByName(string name)
        {
            ObjMetadataRecord? rec = null;
            if (TryGetAllByName(name, out List<ObjMetadataRecord>? f))
            {
                rec = f.First();
            }
            return rec;
        }
        public List<ObjMetadataRecord>? GetAllByName(string name)
        {
            List<ObjMetadataRecord>? f;
            TryGetAllByName(name, out f);
            return f;
        }
        public bool TryGetAllByName(string name, out List<ObjMetadataRecord>? f)
        {
            bool res = _DictByName.TryGetValue(name, out f);
            return res;
        }
        //public Dictionary<int, ObjMetadataRecord> GetRecordsInxDict(string name)
        //{
        //    Dictionary<int, ObjMetadataRecord> dict = new();
        //    ForEach(delegate (ObjMetadataRecord rec)
        //    {
        //        if (rec.Name == name)
        //        {
        //            dict[rec.Index] = rec;
        //        }
        //    });
        //    return dict;
        //}

        //Добавить
        public void Add(ObjMetadataRecord r)
        {
            _List.Add(r);
            List<ObjMetadataRecord>? lstNamed; if(!_DictByName.TryGetValue(r.Name, out lstNamed))
            {
                lstNamed = new List<ObjMetadataRecord>();
                _DictByName[r.Name] = lstNamed;
            }
            lstNamed.Add(r);
        }
    }
}
