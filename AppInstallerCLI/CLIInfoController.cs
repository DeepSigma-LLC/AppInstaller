using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;

namespace AppInstallerCLI
{
    internal static class CLIInfoController
    {
        internal static void ShowInfo()
        {
            Console.WriteLine("App Installer CLI");
            Console.WriteLine("Version: " + AppUtilities.GetAppVersion());
            Console.WriteLine("Current Directory: " + AppUtilities.GetCurrentLocationOfTheAppInstallerApp());
            Console.WriteLine("This is a command-line interface for the App Installer application.");
            Console.WriteLine("For more information, visit the official documentation.");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit(0);
        }

        internal static void InterfaceRequest(string CLIarguement)
        {
            switch (CLIarguement.ToLower())
            {
                case "--version":
                    Console.WriteLine($"AppInstaller Version: {AppUtilities.GetAppVersion()}");
                    break;
                case "--path":
                    Console.WriteLine($"Current Directory: {AppUtilities.GetCurrentLocationOfTheAppInstallerApp()}");
                    break;
                case "--help":
                    Console.WriteLine("Usage: AppInstaller [--version | --help | | --path | No Arguement]");
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
