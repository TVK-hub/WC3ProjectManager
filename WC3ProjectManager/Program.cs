using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Text;
using WC3Files.Common.Ini;
using WC3Files.Objects;
using WC3ProjectManager;
using WC3ProjectManager.Map;
using WC3ProjectManager.Map.Objects;
using WC3ProjectManager.Project;
internal class Program
{
    static void Main(string[] args)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#if DEBUG
        ConsoleEx.ProjectAutoload();
        ConsoleEx.ProgramLoop();

        //WC3Project prj = WC3Project.Current;
        //string gameVer = prj.BaseBuild.GameVersion;
        //foreach (var t in Enum.GetValues<W3objTypes>())
        //{
        //    if (t == W3objTypes.None || t == W3objTypes.All) { continue; }
        //    string dir = prj.Dirs.Objects;
        //    switch (t)
        //    {
        //        case W3objTypes.Unit:
        //            dir += "\\Units";
        //            break;
        //        case W3objTypes.Item:
        //            dir += "\\Items";
        //            break;
        //        case W3objTypes.Ability:
        //            dir += "\\Abilities";
        //            break;
        //        case W3objTypes.Buff:
        //            dir += "\\Buffs";
        //            break;
        //        case W3objTypes.Upgrade:
        //            dir += "\\Upgrades";
        //            break;
        //        case W3objTypes.Doodad:
        //            dir += "\\Doodads";
        //            break;
        //        case W3objTypes.Destructable:
        //            dir += "\\Destructable";
        //            break;
        //    }

        //    //Файлы
        //    FileInfo[] files = new DirectoryInfo(dir).GetFiles("*.ini", SearchOption.AllDirectories);
        //    foreach (FileInfo file in files)
        //    {
        //        IniFile ini = IniFile.Load(file.FullName);
        //        IniEntry en = new IniEntry(ini.Sections[0]);
        //        en.Name = "Type";
        //        en.Value = W3objTypesEx.ToString(t);
        //        ini.Sections[0].Entries.Add(en);
        //        ini.Save(file.FullName);
        //    }
        //}
#else
        try
        {
            ConsoleEx.ProjectAutoload();
            ConsoleEx.ProgramLoop();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
#endif
    }
}