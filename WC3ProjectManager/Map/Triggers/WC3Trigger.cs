using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WC3Files.Triggers;
using WC3ProjectManager.Map.Jass;
namespace WC3ProjectManager.Map.Triggers
{
    public class WC3Trigger : IWC3Trigger
    {
        //Название
        public string Name
        {
            set; get;
        } = "";

        //Описание
        public string Description
        {
            set; get;
        } = "";

        //Код
        public string Code
        {
            set; get;
        } = "";

        //ГУИ
        public List<WtgEca>? GUI = null;

        //Включён
        public bool Enabled = true;
        public bool IsInitiallyOff = false;
        public bool RunOnInit = false;

        //Файл
        public static WC3Trigger FromFile(string path)
        {
            WC3Trigger trg = new WC3Trigger();
            trg.Name = JassUtils.ToTriggerName(Path.GetFileNameWithoutExtension(path));
            trg.Code = File.ReadAllText(path);
            trg.Enabled = true;
            WC3TriggerDirectives.Parse(trg);
            return trg;
        }
        public static WC3Trigger[] FromDir(string dir, bool subfolders = true)
        {
            //Файлы
            SearchOption opt = (subfolders) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var files = Directory.EnumerateFiles(dir, "*", opt).Where(x =>
                    Path.GetExtension(x).Equals(".j", StringComparison.OrdinalIgnoreCase) ||
                    Path.GetExtension(x).Equals(".vj", StringComparison.OrdinalIgnoreCase) ||
                    Path.GetExtension(x).Equals(".cj", StringComparison.OrdinalIgnoreCase)).ToArray();

            //Триггеры
            WC3Trigger[] triggers = new WC3Trigger[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                triggers[i] = FromFile(files[i]);
            }
            return triggers;
        }
        public void Save(string path)
        {
            string code = WC3TriggerDirectives.Generate(this);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, code);
        }

        //В строку
        public override string ToString()
        {
            return Name;
        }
    }
}