using WC3Files.Common.Ini;
namespace WC3ProjectManager.Map.Objects
{
    /// <summary>
    /// Макет файла объектов.
    /// Определяет, в какой секции и в каком порядке располагаются те или иные поля.
    /// </summary>
    public static class WC3ObjectLayout
    {
        //Cловарь
        static Dictionary<string, WC3ObjectFieldLayout> Dict = new();
        public static WC3ObjectFieldLayout GetFieldLayout(WC3ObjectField f)
        {
            string id = (!string.IsNullOrEmpty(f.Id)) ? f.Id : f.Name;
            WC3ObjectFieldLayout layout; if (!Dict.TryGetValue(id, out layout))
            {
                layout = new()
                {
                    Id = id,
                    Index = int.MaxValue,
                    SectionName = "Other",
                    SectionIndex = int.MaxValue
                };
            }
            return layout;
        }

        //Загрузка
        public static void LoadFromIni(string path)
        {
            Dict.Clear();
            IniFile ini = IniFile.Load(path);
            for (int j=0; j < ini.Sections.Count; j++)
            {
                IniSection sec = ini.Sections[j];
                for (int k = 0; k < sec.Entries.Count; k++)
                {
                    IniEntry e = sec.Entries[k];
                    WC3ObjectFieldLayout layout = new();
                    layout.Id = e.Name;
                    layout.Name = e.Value;
                    layout.Index = k;
                    layout.SectionName = sec.Name;
                    layout.SectionIndex = j;
                    Dict[layout.Id] = layout;
                }
            }
        }
    }
}