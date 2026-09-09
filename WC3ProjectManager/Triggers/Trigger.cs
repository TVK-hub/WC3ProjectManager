using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WC3Files.Triggers;
using WC3ProjectManager.Jass;
namespace WC3ProjectManager
{
    public class Trigger : ITrigger
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
        public static Trigger FromFile(string path)
        {
            Trigger trg = new Trigger();
            trg.Name = JassUtils.ToTriggerName(Path.GetFileNameWithoutExtension(path));
            trg.Code = File.ReadAllText(path);
            trg.Enabled = true;
            TriggerDirectives.Parse(trg);
            return trg;
        }
        public static Trigger[] FromDir(string dir, bool subfolders = true)
        {
            //Файлы
            SearchOption opt = (subfolders) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var files = Directory.EnumerateFiles(dir, "*", opt).Where(x =>
                    Path.GetExtension(x).Equals(".j", StringComparison.OrdinalIgnoreCase) ||
                    Path.GetExtension(x).Equals(".vj", StringComparison.OrdinalIgnoreCase)).ToArray();

            //Триггеры
            Trigger[] triggers = new Trigger[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                triggers[i] = FromFile(files[i]);
            }
            return triggers;
        }
        public void Save(string path)
        {
            string code = TriggerDirectives.Generate(this);
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