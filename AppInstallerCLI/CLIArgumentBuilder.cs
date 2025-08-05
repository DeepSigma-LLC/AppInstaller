using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;
using ConsoleLibrary;


namespace AppInstallerCLI
{
    internal static class CLIArgumentBuilder
    {
        internal static ConsoleArgumentCollection GetAllArguments()
        {
            ConsoleArgumentCollection arguments = new();

            ConsoleArgument version = new(() => Console.WriteLine($"Version: {AppUtilities.GetAppVersion()}"),
                "Display the version of the application.");
            arguments.Add("--version", version);

            ConsoleArgument path = new(() => Console.WriteLine($"Current Directory: {AppUtilities.GetCurrentLocationOfTheAppInstallerApp()}"),
               "Display the current directory of the application.");
            arguments.Add("--path", path);

            arguments.Add("No arguments", new ConsoleArgument(() => {}, "Launchs the UI version of the application.")); //Action does nothing, just launches the UI version.

            return arguments;
        }


    }
}
