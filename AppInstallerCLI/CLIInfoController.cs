using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;

namespace AppInstallerCLI
{
    internal class CLIInfoController
    {

        private readonly AppSettings _appSettings;
        public CLIInfoController(AppSettings appSettings)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        internal void ShowInfo()
        {
            Console.WriteLine($"{_appSettings.AppName}");
            Console.WriteLine("Version: " + AppUtilities.GetAppVersion());
            Console.WriteLine("Current Directory: " + AppUtilities.GetCurrentLocationOfTheAppInstallerApp());
            Console.WriteLine($"This is a command-line interface for the {_appSettings.AppName} application.");
            Console.WriteLine("For more information, visit the official documentation.");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit(0);
        }

        internal void InterfaceRequest(string CLIarguement)
        {
            switch (CLIarguement.ToLower())
            {
                case "--version":
                    Console.WriteLine($"{_appSettings.AppName} Version: {AppUtilities.GetAppVersion()}");
                    break;
                case "--path":
                    Console.WriteLine($"Current Directory: {AppUtilities.GetCurrentLocationOfTheAppInstallerApp()}");
                    break;
                case "--help":
                    Console.WriteLine($"Usage: {_appSettings.AppName} [--version | --help | | --path | No Arguement]");
                    Console.WriteLine("--version: Display the version of the application.");
                    Console.WriteLine("--help: Display this help message.");
                    Console.WriteLine("--path: Display the current directory of the application.");
                    Console.WriteLine("No arguement: Launchs the UI version of the application.");
                    break;
                default:
                    Console.WriteLine("Invalid argument");
                    break;
            }
        }
    }
}
