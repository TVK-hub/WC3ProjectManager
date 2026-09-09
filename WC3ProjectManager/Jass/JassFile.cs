using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WC3ProjectManager.Jass
{
    public class JassFile
    {
        //Информация о файле
        private FileInfo FileInfo;
        public string Name{ get => FileInfo.Name; }
        public string Path{ get => FileInfo.FullName; }
        public string? Dir{ get => FileInfo.DirectoryName; }

        //Конструктор
        public JassFile(string path)
        {
            FileInfo = new FileInfo(path);
        }

        //Собрать
        public static JassFile Build(string filePath, Triggers triggers)
        {
            string code = JassUtils.Build(triggers);
            File.WriteAllText(filePath, code);
            return new JassFile(filePath);
        }

        //Проверить
        public bool Check()
        {
            //AdicHelper
            ToolsResult res = Tools.AdicHelper(Path);

            //JassHelper
            if (res.Success)
            {
                Replace(ToolsPaths.AdicHelperParsed);
                //File.AppendAllText(
                //    Path,
                //    "\nfunction main takes nothing returns nothing\n" +
                //    "endfunction\n"
                //);
                res = Tools.JassHelper(Path);
            }

            //Результат
            return res.Success;
        }

        //Удалить
        public void Delete()
        {
            FileInfo.Delete();
        }

        //Заменить
        public void Replace(string srcPath)
        {
            File.Copy(srcPath, Path, true);
        }
    }
}
