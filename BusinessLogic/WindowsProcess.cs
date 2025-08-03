using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public static class WindowsProcess
    {

        /// <summary>
        /// Opens a folder location in your file system.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static void OpenFolder(string folderPath)
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
        public static string? ExecuteCommand(string Command)
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

        public static void ExecuteExeFileDirectly(string FullExeFilePath, string Arguements)
        {
            if (!File.Exists(FullExeFilePath))
            {
                throw new FileNotFoundException($"The executable file was not found: {FullExeFilePath}");
            }

            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = FullExeFilePath,
                Arguments = Arguements,
                WorkingDirectory = Path.GetDirectoryName(FullExeFilePath)!,
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psi);
        }


        /// <summary>
        /// Determine if a program is installed by looking for a valid version response from the terminal.
        /// </summary>
        /// <param name="ProgramName"></param>
        /// <returns></returns>
        public static bool IsProgramInstalled(string programName)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c where {programName}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            string? value = Environment.GetEnvironmentVariable("PATH");

            using (Process? process = Process.Start(startInfo))
            {
                if (process == null) return false;

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                return process.ExitCode == 0 && string.IsNullOrWhiteSpace(error);
            }
        }

        /// <summary>
        /// Returns version from a terminal program name.
        /// Note: This assumes the program supports a `--version` flag to output its version.
        /// </summary>
        /// <param name="ProgramName"></param>
        /// <returns></returns>
        public static string? ProgramInstalledVersion(string ProgramName)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe", //Or powershell.exe
                Arguments = $"/c {ProgramName} --version",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using (Process? process = Process.Start(startInfo))
            {
                if (process is null) return null;

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if(process.ExitCode == 0 && !string.IsNullOrWhiteSpace(output))
                {
                    return output;
                }

                return null;
            }
        }

    }
}
