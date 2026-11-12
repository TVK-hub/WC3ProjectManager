using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WC3ProjectManager.Map.Jass;
using WC3ProjectManager.Map.Triggers;
using WC3ProjectManager.Project;
namespace WC3ProjectManager
{
    /// <summary>
    /// Функции консольного приложения
    /// </summary>
    public static class ConsoleFunc
    {
        public static void Prj(string path)
        {
            bool res = WC3Project.TryLoad(path, out WC3Project.Current);
            if (res)
            {
                Console.WriteLine($"Открыт проект: {WC3Project.Current?.Name}");
                Properties.Settings.Default.LastProjectPath = path;
                Properties.Settings.Default.Save();
            }
            else
            {
                Console.WriteLine($"Не удалось открыть проект по пути: \"{path}\"");
            }
        }
        public static void Extract(string path, WC3DataType dataType)
        {
            WC3Project.Current.Extract(path, dataType);
        }
        public static void Inject(string path, WC3DataType dataType)
        {
            WC3Project.Current.Inject(path, dataType);
        }
        public static void Clear(string path, WC3DataType dataType)
        {
            WC3Project.Current.Clear(path, dataType);
        }
        public static void Replace(string path, WC3DataType dataType)
        {
            Clear(path, dataType);
            Inject(path, dataType);
        }
        public static void Check()
        {
            WC3Project.Current.Check();
        }
    }
}
