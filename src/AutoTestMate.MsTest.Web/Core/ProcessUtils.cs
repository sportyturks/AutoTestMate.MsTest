using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AutoTestMate.MsTest.Web.Core
{
    public enum Platform { UNIX, WIN }
    public class ProcessUtils
    {

        public const string UNIX_PID_REGX = @"\w+\s+(\d+).*";
        public const string WIND_PID_REGX = @".*\s+(\d+)";

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            if (args != null && args.Length == 1)
            {
                findAndKillProcessRuningOn(port: args[0]);
            }
            else
            {
                Console.WriteLine("Illegal port option");
            }
        }

        public static void findAndKillProcessRuningOn(string port)
        {
            var pidList = new List<string>();
            List<string> list;
            switch (GetOsName())
            {
                case Platform.UNIX:
                    list = FindUnixProcess();
                    list = FilterProcessListBy(processList: list, filter: ":" + port);
                    pidList.AddRange(list.Select(pidString => GetPidFrom(pidString: pidString, pattern: UNIX_PID_REGX)).Where(pid => !string.IsNullOrEmpty(pid)));
                    break;

                case Platform.WIN:
                    list = FindWindowsProcess();
                    list = FilterProcessListBy(processList: list, filter: ":" + port);
                    pidList.AddRange(list.Select(pidString => GetPidFrom(pidString: pidString, pattern: WIND_PID_REGX)).Where(pid => !string.IsNullOrEmpty(pid)));
                    break;
                default:
                    Console.WriteLine("No match found");
                    break;
            }

            foreach (var pid in pidList)
            {
                KillProcessBy(pidString: pid);
            }
        }

        public static Platform GetOsName()
        {
            var os = System.Environment.OSVersion.VersionString;
            Console.WriteLine("OS = {0}", os);

            if (os.ToLower().Contains("unix", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.WriteLine("UNIX machine");
                return Platform.UNIX;
            }

            Console.WriteLine("WINDOWS machine");
            return Platform.WIN;
        }

        public static void KillProcessBy(string pidString)
        {
            var pid = -1;
            if (pidString != null && int.TryParse(s: pidString, result: out pid))
            {
                var p = Process.GetProcessById(pid);
                p.Kill();
                Console.WriteLine("Killed pid =" + pidString);
            }
            else
            {
                Console.WriteLine("Process not found for pid =" + pidString);
            }

        }

        public static List<string> FindUnixProcess()
        {
            var processStart = new ProcessStartInfo
            {
                FileName = "bash",
                Arguments = "-c lsof -i",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process();
            process.StartInfo = processStart;
            process.Start();

            var outstr = process.StandardOutput.ReadToEnd();

            return SplitByLineBreak(outstr);
        }

        public static List<string> FindWindowsProcess()
        {
            var processStart = new ProcessStartInfo
            {
                FileName = "netstat.exe",
                Arguments = "-aon",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process();
            process.StartInfo = processStart;
            process.Start();

            var outstr = process.StandardOutput.ReadToEnd();

            return SplitByLineBreak(outstr);
        }

        public static List<string> SplitByLineBreak(string processLines)
        {
            var processList = new List<string>();

            if (processLines != null)
            {
                var list = processLines.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
                processList.AddRange(collection: list);
            }

            return processList;
        }

        public static List<string> FilterProcessListBy(List<string> processList,
                                                   string filter)
        {
            if (processList == null)
            {
                return [];
            }

            return filter == null ? processList : processList.FindAll(i => i != null && i.ToLower().Contains(filter.ToLower()));
        }

        public static String GetPidFrom(String pidString, String pattern)
        {
            var matches = Regex.Matches(pidString, pattern);

            if (matches != null && matches.Count > 0)
            {
                return matches[0].Groups[1].Value;
            }

            return string.Empty;
        }
    }
}
