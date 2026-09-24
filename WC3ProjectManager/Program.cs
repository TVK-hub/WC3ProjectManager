using WC3Files;
using WC3ProjectManager;
using WC3ProjectManager.Cmd;
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
                Command cmd = Command.Parse(line);
                cmd.Execute();
            }
        }
    }
}