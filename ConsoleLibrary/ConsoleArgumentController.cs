using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLibrary
{
    public class ConsoleArgumentController
    {

        private ConsoleArgumentCollection consoleArguments = new();
        private string AppName { get; } = string.Empty;
        public ConsoleArgumentController(ConsoleArgumentCollection consoleArguments, string AppName)
        {
            this.consoleArguments = consoleArguments;
            this.AppName = AppName;
        }


        /// <summary>
        /// Generates a info message for the console application.
        /// </summary>
        /// <param name="AppVersion"></param>
        /// <param name="CurrentInstallationDirectory"></param>
        public void ShowInfo(string AppVersion, string CurrentInstallationDirectory)
        {
            Console.WriteLine($"{AppName}");
            Console.WriteLine($"Version: {AppVersion}");
            Console.WriteLine("Current Directory: " + CurrentInstallationDirectory);
            Console.WriteLine($"This is a command-line interface for the {AppName} application.");
            Console.WriteLine("For more information, visit the official documentation.");
            Environment.Exit(0);
        }

        /// <summary>
        /// Generates a help message for the console application.
        /// </summary>
        /// <param name="AppName"></param>
        public void ShowHelp(string AppName)
        {
            Console.WriteLine($"Usage: {AppName} [--version | --help | | --path | No Arguement]");

            foreach (var argument in consoleArguments.GetCollection())
            {
                Console.WriteLine($"{argument.Key}: {argument.Value.Description}");
            }
        }

        /// <summary>
        /// Processes the command-line arguments passed to the application.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public string[] ProcessArguments(string[] arguments)
        {
            List<string> UI_arguements = [];
            foreach (string arg in arguments)
            {
                string value = arg.Trim();
                if (value.StartsWith("--"))
                {
                    CLIInterfaceRequest(value);
                }
                else if (value.Contains("="))
                {
                    UI_arguements.Add(value);
                }
                else
                {
                    Console.WriteLine($"Unknown argument: {value}");
                }
            }
            return UI_arguements.ToArray();
        }

        /// <summary>
        /// Processes the command-line interface request based on the provided argument.
        /// </summary>
        /// <param name="CLIarguement"></param>
        private void CLIInterfaceRequest(string CLIarguement)
        {
            string arg = CLIarguement.ToLower();
            if (consoleArguments.GetCollection().ContainsKey(arg))
            {
                consoleArguments.GetCollection()[arg].Method.Invoke();
            }
            else
            {
                Console.WriteLine("Invalid argument");
            }
        }

    }
}
