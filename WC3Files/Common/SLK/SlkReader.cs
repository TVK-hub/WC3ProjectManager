using System.Globalization;
using System.Linq.Expressions;
using System.Text;

namespace WC3Files.Common.SLK
{
    public class SlkReader
    {
        //Ридер
        StreamReader Reader;

        //Конструктор
        public SlkReader(string path)
        {
            Reader = new StreamReader(path);
        }

        //Чтение
        public SlkFile ReadFile()
        {
            var f = new SlkFile();
            string[]? parts = null;
            int r = 0, c = 0;
            while ((parts = ReadParts()) != null)
            {
                //Конец файла
                if (parts[0] == "E") { break; }

                //Ячейка
                if (parts[0] == "C")
                {
                    //Разбор частей
                    object? value = null;
                    for (int i = 1; i < parts.Length; i++)
                    {
                        string part = parts[i];
                        switch (part[0])
                        {
                            //Позиция
                            case 'X':
                                int xNew; if (int.TryParse(part.AsSpan(1), out xNew))
                                {
                                    c = xNew-1;
                                }
                                break;
                            case 'Y':
                                int yNew; if (int.TryParse(part.AsSpan(1), out yNew))
                                {
                                    r = yNew-1;
                                }
                                break;
                            //Значение
                            case 'K':
                                //Строка
                                string s = part.Substring(1);

                                //Строковое значение
                                if (s[0]=='"')
                                {
                                    value = s.Substring(1, s.Length-2);
                                }
                                //Логическое значение
                                else if (s == "TRUE")
                                {
                                    value = true;
                                }
                                else if (s == "FALSE")
                                {
                                    value = false;
                                }
                                //Неверное значение
                                else if (s == "#VALUE!")
                                {
                                    value = null;
                                }
                                //Числовое значение
                                else if (s.IndexOf('.') == -1)
                                {
                                    value = int.Parse(s, CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    value = decimal.Parse(s, CultureInfo.InvariantCulture);
                                }
                                break;
                        }
                    }

                    //Добавить запись
                    if (value != null)
                    {
                        f[r, c] = value;
                        f.RowsCount = Math.Max(f.RowsCount, r+1);
                        f.ColumnsCount = Math.Max(f.ColumnsCount, c+1);
                    }
                }
            }
            return f;
        }
        private string[]? ReadParts()
        {
            List<string>? parts = null;
            string? line = Reader.ReadLine();
            if (line != null)
            {
                parts = new List<string>();
                StringBuilder token = new();
                bool quotes = false;
                for (int i = 0; i < line.Length; i++)
                {
                    //Символ
                    char chr = (char)line[i];

                    //Кавычки
                    if (chr == '"')
                    {
                        token.Append(chr);
                        if (!quotes)
                        {
                            quotes = true;
                        }
                        else
                        {
                            quotes = false;
                            parts.Add(token.ToString());
                            token.Clear();
                        }
                    }
                    //Точка с запятой
                    else if (chr == ';' && !quotes)
                    {
                        parts.Add(token.ToString());
                        token.Clear();
                    }
                    //Обычный текст
                    else
                    {
                        token.Append(chr);
                    }
                }
                if (token.Length>0) { parts.Add(token.ToString()); }
            }
            return parts?.ToArray();
        }
    }
}
