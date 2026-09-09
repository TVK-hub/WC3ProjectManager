using WC3Files;
using WC3ProjectManager;
internal class Program
{
    static void Main(string[] args)
    {
        //Метаданные
        WC3Metadata.Load();

        //Консоль
        while (true)
        {
            //Вввод
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) continue;
            string[] lines = input.Split('\n');

            //Обработка
            foreach (string line in lines)
            {
                Parse(line);
            }
        }
    }
    public static void Parse(string input)
    {
        //Части
        string[] inputParts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        //Команда
        string cmd = inputParts[0];
        switch (cmd)
        {
            //Проект
            case "prj":
                {
                    string path = input.Substring(cmd.Length + 1);
                    WC3Project.Current = WC3Project.Load(path);
                }
                break;
            //Извлечение
            case "extract":
                {
                    string path = ""; if (inputParts.Length > 1) { path = input.Substring(cmd.Length + 1); }
                    WC3Project.Current.Extract();
                }
                break;
            //Внедрение
            case "inject":
                {
                    WC3Project.Current.Inject();
                }
                break;
            //Очистка
            case "clear":
                {
                    WC3Project.Current.Clear();
                }
                break;
            //Проверка
            case "check":
                {
                    WC3Project.Current.Check();
                }
                break;
        }
    }
}