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
                string? currentLocation = AppUtilities.GetCurrentLocationOfTheAppInstallerApp();
                if (currentLocation is null) { return; }

                string? UI_Path = Path.GetDirectoryName(currentLocation); //Returns CLI directory path
                UI_Path = Path.GetDirectoryName(UI_Path); //Returns Base directory path
                if (UI_Path is null) { return; }

                UI_Path = Path.Combine(UI_Path, "Main", "AppInstallerUI.exe");
                WindowsProcess.ExecuteExeFileDirectly(UI_Path, string.Empty);
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
