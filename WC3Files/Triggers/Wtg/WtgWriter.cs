using System.Reflection.PortableExecutable;

namespace WC3Files.Triggers
{
    public class WtgWriter
    {
        //Ридер
        WC3Writer Writer;

        //Конструктор
        public WtgWriter(MemoryStream stream)
        {
            Writer = new WC3Writer(stream);
        }

        //Запись
        public void WriteFile(WtgFile wtg)
        {
            //Заголовок
            Writer.WriteFxString("WTG!");
            Writer.Write(wtg.Version);

            //Категории
            Writer.Write(wtg.Categories.Count);
            foreach (WtgCategory cat in wtg.Categories)
            {
                WriteCategory(cat);
            }

            //Версия игры
            Writer.Write(wtg.GameVersion);

            //Глобальные переменные
            Writer.Write(wtg.GlobalVars.Count);
            foreach (WtgVariable var in wtg.GlobalVars)
            {
                WriteVariable(var);
            }

            //Триггеры
            Writer.Write(wtg.Triggers.Count);
            foreach (WtgTrigger t in wtg.Triggers)
            {
                WriteTrigger(t);
            }
        }
        public void WriteCategory(WtgCategory cat)
        {
            Writer.Write(cat.Index);
            Writer.WriteCString(cat.Name);
            Writer.Write(cat.IsComment?1:0);
        }
        public void WriteVariable(WtgVariable var)
        {
            Writer.WriteCString(var.Name);
            Writer.WriteCString(var.Type);
            Writer.Write(var.Unknown);
            Writer.Write(var.IsArray?1:0);
            Writer.Write(var.ArraySize);
            Writer.Write(var.IsInitialized?1:0);
            Writer.WriteCString(var.Value);
        }
        public void WriteTrigger(WtgTrigger t)
        {
            //Тригер
            Writer.WriteCString(t.Name);
            Writer.WriteCString(t.Description);
            Writer.Write(t.IsComment ? 1:0);
            Writer.Write(t.IsEnabled ? 1:0);
            Writer.Write(t.IsCustomText ? 1:0);
            Writer.Write(t.IsInitiallyOff ? 1:0);
            Writer.Write(t.RunOnInit ? 1:0);
            Writer.Write(t.CategoryId);

            //События/Условия/Действия
            Writer.Write(t.ECA.Count);
            foreach (WtgEca eca in t.ECA)
            {
                WriteEca(eca, false);
            }
        }
        public void WriteEca(WtgEca eca, bool isChild)
        {
            //ECA
            Writer.Write((int)eca.Type);
            if (isChild) Writer.Write((int)eca.Group);
            Writer.WriteCString(eca.Name);
            Writer.Write(eca.IsEnabled ? 1:0);

            //Параметры
            int prmsCount = WC3Metadata.Trigger.GetFunction(eca.Name).Parameters.Count;
            foreach (WtgParameter p in eca.Parameters)
            {
                WriteParameter(p);
            }

            //Дочерние ECA
            Writer.Write(eca.Childrens.Count);
            foreach (WtgEca ch in eca.Childrens)
            {
                WriteEca(ch, true);
            }
        }
        public void WriteParameter(WtgParameter p)
        {
            //Параметр
            Writer.Write((int)p.Type);
            Writer.WriteCString(p.Value);

            //Подпараметры
            Writer.Write(p.SubParameters!=null?1:0);
            if (p.SubParameters != null)
            {
                WriteSubParameters(p.SubParameters);
                Writer.Write(0);
            }

            //Массив
            Writer.Write(p.IsArray?1:0);
            if (p.IsArray) { WriteParameter(p.ArrayIndex); }
        }
        public void WriteSubParameters(WtgSubParameters p)
        {
            //Подпараметры
            Writer.Write((int)p.Type);
            Writer.WriteCString(p.Name);

            //Параметры
            Writer.Write(p.Parameters!=null?1:0);
            if (p.Parameters != null)
            {
                foreach (WtgParameter param in p.Parameters)
                {
                    WriteParameter(param);
                }
            }
        }
    }
}
