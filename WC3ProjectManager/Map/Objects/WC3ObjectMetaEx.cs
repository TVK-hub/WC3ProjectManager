using WC3Files.Metadata.Objects;
using WC3Files.Objects;
namespace WC3ProjectManager.Map.Objects
{
    public static class WC3ObjectMetaEx
    {
        //По объекту
        public static ObjMetadataRecord? GetByObject(this ObjMetadataRecords lst, WC3Object? obj, string fName)
        {
            return lst.GetAllByObject(obj, fName)?.FirstOrDefault();
        }
        public static List<ObjMetadataRecord>? GetAllByObject(this ObjMetadataRecords lst, WC3Object? obj, string fName)
        {
            List<ObjMetadataRecord>? recs = null;
            if (lst.TryGetAllByName(fName, out recs) && recs.Count > 1)
            {
                //Юнит
                if (obj.WC3Type == W3objTypes.Unit)
                {
                    //Герой
                    if (char.IsUpper(obj.Id[0]))
                    {
                        recs = recs.FindAll(itm => (int?)itm.GetFieldValue("useHero") == 1);
                    }
                    //Здание
                    else if (obj.Fields.TryGetByIdLvl("ubdg", 0, out WC3ObjectField? fIsBld) && (int?)fIsBld.Value == 1)
                    {
                        recs = recs.FindAll(itm => (int?)itm.GetFieldValue("useBuilding") == 1);
                    }
                    //Обычный
                    else
                    {
                        recs = recs.FindAll(itm => (int?)itm.GetFieldValue("useUnit") == 1);
                    }
                }
                //Предмет
                else if (obj.WC3Type == W3objTypes.Item)
                {
                    recs = recs.FindAll(itm => (int?)itm.GetFieldValue("useItem") == 1);
                }
            }
            return recs;
        }
    }
}
