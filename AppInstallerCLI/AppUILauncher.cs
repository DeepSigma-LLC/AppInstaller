using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;

namespace AppInstallerCLI
{
    internal static class AppUILauncher
    {
        internal static void LaunchAppUI(string? AppName = null, string? SourcePath = null, string? TargetPath = null)
        {
            try
            {
                // Attempt to launch the App Installer UI
                //WindowsProcess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                // If an error occurs, log it and exit the application
                Console.WriteLine("Error launching App Installer UI: " + ex.Message);
                Environment.Exit(1);
            }
        }
    }
}
