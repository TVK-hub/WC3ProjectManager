using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WC3ProjectManager
{
    public static class Tools
    {
        public static ToolsResult AdicHelper(string scrPath)
        {
            //Аргументы
            string arguments =
            $"/tmcrpre=\"{ToolsPaths.JassHelperCli}\" " +
            "/alf " +
            "/mcm " +
            "/ibj=\"0\" " +
            "/icj=\"0\" " +
            "/dbt " +
            $"/scrpars=\"{scrPath}\"";

            //Запуск
            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = ToolsPaths.AdicHelper,
                WorkingDirectory = ToolsPaths.AdicHelperDir,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            using Process process = Process.Start(info)!;

            //Результаты
            ToolsResult res = new();
            res.Output = process.StandardOutput.ReadToEnd();
            res.Error = process.StandardError.ReadToEnd();
            process.WaitForExit();
            res.ExitCode = process.ExitCode;
            return res;
        }
        public static ToolsResult JassHelper(string scrPath)
        {
            //Аргументы
            string arguments =
            "--nooptimize "+
            "--scriptonly "+
            $"{ToolsPaths.JassHelperDir}\\common.j " +
            $"{ToolsPaths.JassHelperDir}\\Blizzard.j " +
            $"\"{scrPath}\" " +
            $"\"{scrPath}\"";

            //Запуск
            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = ToolsPaths.JassHelper,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            using Process process = Process.Start(info)!;

            //Результаты
            ToolsResult res = new();
            res.Output = process.StandardOutput.ReadToEnd();
            res.Error = process.StandardError.ReadToEnd();
            process.WaitForExit();
            res.ExitCode = process.ExitCode;
            return res;
        }
    }
    public class ToolsResult
    {
        public bool Success { get => ExitCode == 0; }
        public int ExitCode { get; set; }
        public string Output { get; set; } = "";
        public string Error { get; set; } = "";
    }
    public static class ToolsPaths
    {
        public static readonly string AdicHelperDir = AppContext.BaseDirectory + "Tools\\AdicHelper";
        public static readonly string AdicHelper = AdicHelperDir + "\\AdicHelper.exe";
        public static readonly string AdicHelperParsed = AdicHelperDir + "\\parsed_war3map.j";

        public static readonly string JassHelperDir = AppContext.BaseDirectory + "Tools\\JassHelper";
        public static readonly string JassHelper = JassHelperDir + "\\JassHelper.exe";
        public static readonly string JassHelperCli = JassHelperDir + "\\clijasshelper.exe";
    }
}
