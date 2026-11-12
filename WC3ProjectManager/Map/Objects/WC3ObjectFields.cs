
namespace WC3ProjectManager.Map.Objects
{
    public class WC3ObjectFields
    {
        //Список
        private List<WC3ObjectField> _List = new();
        public Dictionary<(string Id, int Level), WC3ObjectField>.ValueCollection Values
        {
            get
            {
                return _DictByIdLvl.Values;
            }
        }
        public Dictionary<(string Id, int Level), WC3ObjectField>.KeyCollection Keys
        {
            get
            {
                return _DictByIdLvl.Keys;
            }
        }
        public WC3ObjectField this[string id, int lvl]
        {
            get
            {
                return _DictByIdLvl[(id, lvl)];
            }
            set
            {
                _DictByIdLvl[(id, lvl)] = value;
                _DictByName[(value.Name, lvl)] = value;
            }
        }

        //По Id и уровню
        private Dictionary<(string Id, int Level), WC3ObjectField> _DictByIdLvl = new();
        public WC3ObjectField? GetByIdLvl(string id, int lvl)
        {
            WC3ObjectField? f;
            TryGetByIdLvl(id, lvl, out f);
            return f;
        }
        public bool TryGetByIdLvl(string id, int lvl, out WC3ObjectField? f)
        {
            return _DictByIdLvl.TryGetValue((id, lvl), out f);
        }

        //По имени
        private Dictionary<(string Name, int Level), WC3ObjectField> _DictByName = new();
        public WC3ObjectField? GetByName(string name)
        {
            WC3ObjectField? f;
            TryGetByName(name, out f);
            return f;
        }
        public bool TryGetByName(string name, out WC3ObjectField? f)
        {
            bool res = _DictByName.TryGetValue((name, 0), out f);
            if (!res) res = _DictByName.TryGetValue((name, 1), out f);
            return res;
        }

        //Создать
        public WC3ObjectField Create(string id, string name, object? value, int lvl)
        {
            WC3ObjectField f = new WC3ObjectField()
            {
                Id = id,
                Name = name,
                Value = value,
                Level = lvl
            };
            this[id, lvl] = f;
            return f;
        }
    }
}
