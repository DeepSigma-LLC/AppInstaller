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

        /// <summary>
        /// Opens a folder location in your file system.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <exception cref="DirectoryNotFoundException"></exception>
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

        /// <summary>
        /// Executes command in command terminal.
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        internal static string? ExecuteCommand(string Command)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe", //Or powershell.exe
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


        /// <summary>
        /// Determine if a program is installed by looking for a valid version response from the terminal.
        /// </summary>
        /// <param name="ProgramName"></param>
        /// <returns></returns>
        internal static bool IsProgramInstalled(string ProgramName)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe", //Or powershell.exe
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

        /// <summary>
        /// Returns version from a terminal program name.
        /// </summary>
        /// <param name="ProgramName"></param>
        /// <returns></returns>
        internal static string? ProgramInstalledVersion(string ProgramName)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe", //Or powershell.exe
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

        /// <summary>
        /// Gets an array of the most common terminal response error messages.
        /// </summary>
        /// <returns></returns>
        private static string[] GetErrorResponses()
        {
            return ["error", "not recognized", "not found", "exception"];
        }
    }
}
