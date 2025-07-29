using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppInstaller.Classes
{
    internal static class WindowsProcess
    {
        internal static async Task<string?> ExecuteCommand(string Command, string file_name = "cmd.exe")
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = file_name, //Or powershell.exe
                Arguments = Command,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using (Process? process = Process.Start(startInfo))
            {
                if (process is null) return null;

                string output = process.StandardOutput.ReadToEnd();
                await process.WaitForExitAsync();
                return output;
            }
        }


        internal static async Task<bool> IsProgramInstalled(string ProgramName, string file_name = "cmd.exe")
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = file_name, //Or powershell.exe
                Arguments = ProgramName + " --version",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using (Process? process = Process.Start(startInfo))
            {
                if (process is null) return false;

                string output = process.StandardOutput.ReadToEnd();
                await process.WaitForExitAsync();

                foreach(string error in GetErrorResponses())
                {
                    if (output.Contains(error))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        internal static async Task<string?> ProgramInstalledVersion(string ProgramName, string file_name = "cmd.exe")
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = file_name, //Or powershell.exe
                Arguments = ProgramName + " --version",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using (Process? process = Process.Start(startInfo))
            {
                if (process is null) return null;

                string output = process.StandardOutput.ReadToEnd();
                await process.WaitForExitAsync();

                foreach (string error in GetErrorResponses())
                {
                    if (output.Contains(error))
                    {
                        return null;
                    }
                }
                return output;
            }
        }

        private static string[] GetErrorResponses()
        {
            return ["error", "not recognized", "not found", "exception"];
        }
    }
}
