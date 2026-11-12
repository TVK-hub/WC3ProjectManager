namespace WC3Files.Objects
{
    public class W3objReader
    {
        //Ридер
        WC3Reader Reader;

        //Тип объектов
        W3objTypes ObjType;

        //Конструктор
        public W3objReader(byte[] data, W3objTypes objType)
        {
            Reader = new WC3Reader(data);
            ObjType = objType;
        }

        //Чтение
        public W3objFile ReadFile()
        {
            //Класс
            var f = new W3objFile();

            //Версия
            f.Version = Reader.ReadInt32();

            //Изменения стандартных объектов
            int standCount = Reader.ReadInt32();
            for (int i = 0; i < standCount; i++)
            {
                W3objMod m = ReadMod();
                f.StandartMods.Add(m);
            }

            //Изменения нестандартных объектов
            int customCount = Reader.ReadInt32();
            for (int i = 0; i < customCount; i++)
            {
                W3objMod m = ReadMod();
                f.CustomMods.Add(m);
            }
            return f;
        }
        public W3objMod ReadMod()
        {
            //Исполнение
            W3objMod mod = new W3objMod();

            //Id
            mod.BaseId = Reader.ReadFxString(4);
            mod.CustomId = Reader.ReadFxString(4);

            //Поля
            if (mod.BaseId == "ntav") { }
            int count = Reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                W3objField f = ReadField();
                mod.Fields.Add(f);
            }
            return mod;
        }
        public W3objField ReadField()
        {
            //Поле
            W3objField f = new W3objField();
            f.Id = Reader.ReadFxString(4); while (f.Id[f.Id.Length - 1] == '\0') { f.Id = f.Id.Remove(f.Id.Length - 1); }
            f.ValueType = (W3objValueType)Reader.ReadInt32();
            if (ObjType == W3objTypes.Doodad || ObjType == W3objTypes.Ability || ObjType == W3objTypes.Upgrade)
            {
                f.Level = Reader.ReadInt32();
                f.DataPointer = Reader.ReadInt32();
            }

            //Значение
            switch (f.ValueType)
            {
                case W3objValueType.Int:
                    f.Value = Reader.ReadInt32();
                    break;
                case W3objValueType.Real:
                case W3objValueType.Unreal:
                    f.Value = Reader.ReadSingle();
                    break;
                case W3objValueType.String:
                    f.Value = Reader.ReadCString();
                    break;
            }

            //Конец
            Reader.ReadFxString(4);
            return f;
        }
    }
}
