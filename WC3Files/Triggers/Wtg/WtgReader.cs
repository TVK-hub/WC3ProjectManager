using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WC3Files.Triggers
{
    public class WtgReader
    {
        //Ридер
        WC3Reader Reader;

        //Конструктор
        public WtgReader(byte[] data)
        {
            Reader = new WC3Reader(data);
        }

        //Чтение
        public WtgFile ReadFile()
        {
            //Класс
            var wtg = new WtgFile();

            //Заголовок
            string magic = Reader.ReadFxString(4);
            wtg.Version = Reader.ReadInt32();

            //Категории
            int catCount = Reader.ReadInt32();
            for (int i = 0; i < catCount; i++)
            {
                WtgCategory cat = ReadCategory();
                wtg.Categories.Add(cat);
            }

            //Версия игры
            wtg.GameVersion = Reader.ReadInt32();

            //Глобальные переменные
            int varsCount = Reader.ReadInt32();
            for (int i = 0; i < varsCount; i++)
            {
                WtgVariable var = ReadVariable();
                wtg.GlobalVars.Add(var);
            }

            //Триггеры
            int trgCount = Reader.ReadInt32();
            for (int i = 0; i < trgCount; i++)
            {
                WtgTrigger t = ReadTrigger();
                wtg.Triggers.Add(t);
            }

            //Вернуть
            return wtg;
        }
        public WtgCategory ReadCategory()
        {
            WtgCategory cat = new WtgCategory();
            cat.Index = Reader.ReadInt32();
            cat.Name = Reader.ReadCString();
            cat.IsComment = Reader.ReadInt32() == 1;
            return cat;
        }
        public WtgVariable ReadVariable()
        {
            WtgVariable var = new WtgVariable();
            var.Name = Reader.ReadCString();
            var.Type = Reader.ReadCString();
            var.Unknown = Reader.ReadInt32();
            var.IsArray = Reader.ReadInt32() == 1;
            var.ArraySize = Reader.ReadInt32();
            var.IsInitialized = Reader.ReadInt32() == 1;
            var.Value = Reader.ReadCString();
            return var;
        }
        public WtgTrigger ReadTrigger()
        {
            //Тригер
            WtgTrigger t = new WtgTrigger();
            t.Name = Reader.ReadCString();
            t.Description = Reader.ReadCString();
            t.IsComment = Reader.ReadInt32()>0;
            t.IsEnabled = Reader.ReadInt32()==1;
            t.IsCustomText = Reader.ReadInt32()==1;
            t.IsInitiallyOff = Reader.ReadInt32()==1;
            t.RunOnInit = Reader.ReadInt32()==1;
            t.CategoryId = Reader.ReadInt32();

            //События/Условия/Действия
            int ecaCount = Reader.ReadInt32();
            for (int i = 0; i < ecaCount; i++)
            {
                WtgEca eca = ReadEca(false);
                t.ECA.Add(eca);
            }
            return t;
        }
        public WtgEca ReadEca(bool isChild)
        {
            //ECA
            WtgEca eca = new WtgEca();
            eca.Type = (WtgEcaType)Reader.ReadInt32();
            if (isChild) eca.Group = (WtgEcaGroup)Reader.ReadInt32();
            eca.Name = Reader.ReadCString();
            eca.IsEnabled = Reader.ReadInt32() == 1;

            //Параметры
            int prmsCount = WC3Metadata.Trigger.GetFunction(eca.Name).Parameters.Count;
            for (int i = 0; i < prmsCount; i++)
            {
                WtgParameter p = ReadParameter();
                eca.Parameters.Add(p);
            }

            //Дочерние ECA
            int childsCount = Reader.ReadInt32();
            for (int i = 0; i < childsCount; i++)
            {
                WtgEca ch = ReadEca(true);
                eca.Childrens.Add(ch);
            }
            return eca;
        }
        public WtgParameter ReadParameter()
        {
            //Параметр
            WtgParameter p = new WtgParameter();
            p.Type = (WtgParameterTypes)Reader.ReadInt32();
            p.Value = Reader.ReadCString();

            //Подпараметры
            bool hasSubParams = Reader.ReadInt32()==1;
            if (hasSubParams)
            {
                p.SubParameters = ReadSubParameters();
                int unknown = Reader.ReadInt32();
            }

            //Массив
            p.IsArray = Reader.ReadInt32() == 1;
            if (p.IsArray) { p.ArrayIndex = ReadParameter(); }
            return p;
        }
        public WtgSubParameters ReadSubParameters()
        {
            //Подпараметры
            WtgSubParameters p = new WtgSubParameters();
            p.Type = (WtgParameterTypes)Reader.ReadInt32();
            p.Name = Reader.ReadCString();

            //Параметры
            bool hasParams = Reader.ReadInt32() > 0;
            if (hasParams)
            {
                int len = WC3Metadata.Trigger.GetFunction(p.Name).Parameters.Count;
                p.Parameters = new WtgParameter[len];
                for (int i = 0; i < len; i++)
                {
                    p.Parameters[i] = ReadParameter();
                }
            }
            return p;
        }
    }
}
