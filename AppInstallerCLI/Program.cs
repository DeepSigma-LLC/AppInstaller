// See https://aka.ms/new-console-template for more information
// Program.cs
using AppInstallerCLI;
using BusinessLogic;
using Microsoft.Extensions.Configuration;
using ConsoleLibrary;


string[] original_args = args; // Store original arguments for CLI processing

// Set up configuration
AppSettings settings = new();
var config = new ConfigurationBuilder()
    .SetBasePath(AppUtilities.GetCurrentLocationOfTheAppInstallerApp())
    .AddJsonFile("config.json", optional: false, reloadOnChange: true)
    .Build();
config.GetSection("AppSettings").Bind(settings);

ConsoleArgumentCollection argumentsMetaData = new();
ConsoleArgumentController CLIController = new(CLIArgumentBuilder.GetAllArguments(), settings.AppName);


List<string> UI_arguements = [];
if (args.Length > 0)
{
    UI_arguements.AddRange(CLIController.ProcessArguments(args));
}


if (original_args.Length == 0 || UI_arguements.Count() > 0)
{
    Console.WriteLine($"Launching {settings.AppName} UI...");
    Console.WriteLine($"Arguments: {string.Join(", ", UI_arguements)}");

    string arg_text = string.Join(" ", UI_arguements);
    AppUILauncher appUI = new(settings);
    appUI.LaunchAppUI(arg_text);
    CLIController.ShowInfo(AppUtilities.GetAppVersion(), AppUtilities.GetCurrentLocationOfTheAppInstallerApp());
}


