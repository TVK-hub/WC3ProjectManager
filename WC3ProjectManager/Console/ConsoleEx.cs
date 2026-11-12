namespace WC3ProjectManager
{
    public static class ConsoleEx
    {
        public static void ProjectAutoload()
        {
            string? path = Properties.Settings.Default.LastProjectPath;
            if (!string.IsNullOrEmpty(path))
            {
                ConsoleFunc.Prj(path);
            }
        }
        public static void ProgramLoop()
        {
            while (true)
            {
                //Вввод
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;
                string[] lines = input.Split('\n');

                //Обработка
                foreach (string line in lines)
                {
                    ConsoleCmd cmd = ConsoleCmd.Parse(line);
                    cmd.Execute();
                }
            }
        }
    }
}
