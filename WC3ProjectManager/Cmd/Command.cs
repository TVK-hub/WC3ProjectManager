namespace WC3ProjectManager.Cmd
{
    public class Command
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
        public Command(string name)
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
                        WC3Project.Current = WC3Project.Load(Path);
                    }
                    break;
                //Извлечение
                case "extract":
                    {
                        WC3Project.Current.Extract(Path, DataType);
                    }
                    break;
                //Внедрение
                case "inject":
                    {
                        WC3Project.Current.Inject(Path, DataType);
                    }
                    break;
                //Очистка
                case "clear":
                    {
                        WC3Project.Current.Clear(Path, DataType);
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

        //Разбор
        public static Command Parse(string input)
        {
            //Части
            string[] inputParts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            //Команда
            Command cmd = new Command(inputParts[0]);

            //Аргументы
            bool pathReading = false;
            for (int i = 1; i < inputParts.Length; i++)
            {
                string part = inputParts[i];
                switch (part)
                {
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
