using System.Text;
using WC3ProjectManager;
internal class Program
{
    static void Main(string[] args)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#if DEBUG
        ConsoleEx.ProjectAutoload();
        ConsoleEx.ProgramLoop();
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