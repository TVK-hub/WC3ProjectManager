using System.Data;
using System.Reflection.PortableExecutable;

namespace WC3Files.Objects
{
    public class W3objWriter
    {
        //Ридер
        WC3Writer Writer;

        //Тип объектов
        W3objTypes ObjType;

        //Конструктор
        public W3objWriter(MemoryStream stream, W3objTypes objType)
        {
            Writer = new WC3Writer(stream);
            ObjType = objType;
        }

        //Запись
        public void WriteFile(W3objFile f)
        {
            //Версия
            Writer.Write(f.Version);

            //Изменения стандартных объектов
            Writer.Write(f.StandartMods.Count);
            foreach (W3objMod mod in f.StandartMods)
            {
                WriteMode(mod);
            }

            //Изменения нестандартных объектов
            Writer.Write(f.CustomMods.Count);
            foreach (W3objMod mod in f.CustomMods)
            {
                WriteMode(mod);
            }
        }
        public void WriteMode(W3objMod mod)
        {
            //Id
            Writer.WriteFxString(mod.BaseId);
            Writer.WriteFxString(mod.CustomId);
            if (mod.BaseId == "ntav") { }

            //Поля
            Writer.Write(mod.Fields.Count);
            foreach (W3objField f in mod.Fields)
            {
                WriteField(f);
            }
        }
        public void WriteField(W3objField f)
        {
            //Поле
            Writer.WriteFxString(f.Id, 4);
            Writer.Write((int)f.ValueType);
            if (ObjType == W3objTypes.Doodad || ObjType == W3objTypes.Ability || ObjType == W3objTypes.Upgrade)
            {
                Writer.Write(f.Level);
                Writer.Write(f.DataPointer);
            }

            //Значение
            switch (f.ValueType)
            {
                case W3objValueType.Int:
                    Writer.Write((int)f.Value);
                    break;
                case W3objValueType.Real:
                case W3objValueType.Unreal:
                    Writer.Write((float)f.Value);
                    break;
                case W3objValueType.String:
                    Writer.WriteCString((string)f.Value);
                    break;
            }

            //Конец
            Writer.WriteFxString(f.Id, 4);
        }
    }
}