using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;

namespace AppInstallerCLI
{
    internal class AppUILauncher
    {
        private readonly AppSettings _appSettings;
        public AppUILauncher(AppSettings appSettings)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }
        internal void LaunchAppUI(string? arguments)
        {
            try
            {
                // Attempt to launch the App Installer UI
                string? currentLocation = AppUtilities.GetCurrentLocationOfTheAppInstallerApp();
                if (currentLocation is null) { return; }

                string? UI_Path = Path.GetDirectoryName(currentLocation); //Returns CLI directory path
                UI_Path = Path.GetDirectoryName(UI_Path); //Returns Base directory path
                if (UI_Path is null) { return; }

                UI_Path = Path.Combine(UI_Path, _appSettings.MainDirectory, "AppInstallerUI.exe");

                WindowsProcess.ExecuteExeFileDirectly(UI_Path, arguments ?? string.Empty);
            }
            catch (Exception ex)
            {
                // If an error occurs, log it and exit the application
                Console.WriteLine($"Error launching {_appSettings.AppName} UI: " + ex.Message);
                Environment.Exit(1);
            }
        }
    }
}
