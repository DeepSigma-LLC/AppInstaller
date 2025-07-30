using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppInstaller.Classes
{
    internal static class WindowsProcess
    {

        internal static void OpenFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = folderPath,
                    UseShellExecute = true // Must be true to open in Explorer
                });
            }
            else
            {
                // Optionally show a message or handle the error
                Debug.WriteLine("Folder does not exist.");
                throw new DirectoryNotFoundException($"Directory not found: {folderPath}");
            }
        }

        internal static string? ExecuteCommand(string Command, string file_name = "cmd.exe")
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = file_name, //Or powershell.exe
                Arguments = $"/c {Command}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using (Process? process = Process.Start(startInfo))
            {
                if (process is null) return null;

                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return output;
            }
        }


        internal static bool IsProgramInstalled(string ProgramName, string file_name = "cmd.exe")
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = file_name, //Or powershell.exe
                Arguments = $"/c {ProgramName} --version",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using (Process? process = Process.Start(startInfo))
            {
                if (process is null) return false;

                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

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

        internal static string? ProgramInstalledVersion(string ProgramName, string file_name = "cmd.exe")
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = file_name, //Or powershell.exe
                Arguments = $"/c {ProgramName} --version",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using (Process? process = Process.Start(startInfo))
            {
                if (process is null) return null;

                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

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
