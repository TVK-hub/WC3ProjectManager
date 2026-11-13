using WC3ProjectManager.Project;
namespace WC3ProjectManager
{
    /// <summary>
    /// Команда консольного приложения
    /// </summary>
    public class ConsoleCmd
    {
        //Имя
        public string Name
        {
            get;
            private set;
        }

        //Аргументы
        public string Path
        {
            get;
            private set;
        } = "";
        public WC3DataType DataType
        {
            get;
            private set;
        } = 0;

        //Конструктор
        public ConsoleCmd(string name)
        {
            Name = name.ToLower();
        }

        //Выполнение
        public void Execute()
        {
            switch (Name)
            {
                //Проект
                case "prj":
                    {
                        ConsoleFunc.Prj(Path);
                    }
                    break;
                //Извлечение
                case "extract":
                    {
                        ConsoleFunc.Extract(Path, DataType);
                    }
                    break;
                //Внедрение
                case "inject":
                    {
                        ConsoleFunc.Inject(Path, DataType);
                    }
                    break;
                //Очистка
                case "clear":
                    {
                        ConsoleFunc.Clear(Path, DataType);
                    }
                    break;
                //Замена
                case "replace":
                    {
                        ConsoleFunc.Replace(Path, DataType);
                    }
                    break;
                //Проверка
                case "check":
                    {
                        ConsoleFunc.Check();
                    }
                    break;
            }
        }

        //Разбор
        public static ConsoleCmd Parse(string input)
        {
            //Части
            string[] inputParts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            //Команда
            ConsoleCmd cmd = new ConsoleCmd(inputParts[0]);

            //Аргументы
            bool pathReading = false;
            for (int i = 1; i < inputParts.Length; i++)
            {
                string part = inputParts[i];
                switch (part)
                {
                    case "-c":
                        cmd.DataType |= WC3DataType.Config;
                        pathReading = false;
                        break;
                    case "-o":
                        cmd.DataType |= WC3DataType.Objects;
                        pathReading = false;
                        break;
                    case "-t":
                        cmd.DataType |= WC3DataType.Triggers;
                        pathReading = false;
                        break;
                    case "-i":
                        cmd.DataType |= WC3DataType.Import;
                        pathReading = false;
                        break;
                    default:
                        //Путь ещё не читается
                        if (!pathReading)
                        {
                            //Начать чтение
                            if (string.IsNullOrEmpty(cmd.Path))
                            {
                                cmd.Path = part;
                                pathReading = true;
                            }
                            //Путь уже был прочитан
                            else
                            {
                                throw new Exception("Неверный синтаксис команды.");
                            }
                        }
                        //Путь читается
                        else
                        {
                            cmd.Path += " " + part;
                        }
                        break;
                }
            }
            if (cmd.DataType == 0) { cmd.DataType = WC3DataType.All; }
            return cmd;
        }
    }
}
