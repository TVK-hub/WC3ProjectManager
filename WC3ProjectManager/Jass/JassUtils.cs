using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace WC3ProjectManager.Jass
{
    public static class JassUtils
    {
        //Сборка
        public static string Build(Triggers triggers)
        {
            //Заголовок
            StringBuilder builder = new();
            builder.AppendLine(triggers.Header.Code);
            builder.AppendLine();

            //Глобальные
            builder.AppendLine("globals");
            foreach (ITrigger t in triggers.All)
            {
                builder.AppendLine("trigger "+ToTriggerVariable(t.Name)+" = null");
            }
            builder.AppendLine("endglobals");
            builder.AppendLine();

            //Триггеры
            foreach (ITrigger trigger in triggers.All)
            {
                if (trigger is Trigger trg && trg.Enabled)
                {
                    //GUI
                    if (trg.GUI != null)
                    {
                        throw new NotSupportedException("Обработка GUI-триггеров ещё не реализована.");
                    }
                    //Код
                    else
                    {
                        builder.AppendLine(trg.Code);
                        builder.AppendLine();
                    }
                }
            }

            //Main
            builder.AppendLine("function main takes nothing returns nothing");
            builder.AppendLine("endfunction");

            //Итоговый код
            return builder.ToString();
        }

        //Строки
        public static string ToTriggerName(string name)
        {
            return Regex.Replace(name, @"^\d+\)", "");
        }
        public static string ToTriggerVariable(string name)
        {
            return "gg_trg_" + name.Replace(' ', '_');
        }
    }
}