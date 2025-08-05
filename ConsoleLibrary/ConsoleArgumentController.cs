using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLibrary
{
    public class ConsoleArgumentController
    {
        private ConsoleArgumentCollection consoleArguments { get; init; }
        private string AppName { get; } = string.Empty;
        public ConsoleArgumentController(ConsoleArgumentCollection consoleArguments, string AppName)
        {
            this.consoleArguments = consoleArguments;
            this.AppName = AppName;
            AddHelpArgument();
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
        private void CLIInterfaceRequest(string argument)
        {
            string arg = argument.ToLower();
            if (consoleArguments.GetCollection().ContainsKey(arg))
            {
                consoleArguments.GetCollection()[arg].Method.Invoke();
            }
            else
            {
                Console.WriteLine("Invalid argument");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Adds a help argument to the console arguments collection.
        /// </summary>
        private void AddHelpArgument()
        {
            consoleArguments.Add("--help", new ConsoleArgument(ShowHelp, ""));
        }


        /// <summary>
        /// Generates a help message for the console application.
        /// </summary>
        /// <param name="AppName"></param>
        private void ShowHelp()
        {
            Console.WriteLine($"Usage: {AppName} [--version | --help | --path | No Argument]");

            foreach (var argument in consoleArguments.GetCollection())
            {
                Console.WriteLine($"{argument.Key}: {argument.Value.Description}");
            }
        }

    }
}
