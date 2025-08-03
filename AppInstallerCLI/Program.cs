// See https://aka.ms/new-console-template for more information
// Program.cs
using AppInstallerCLI;
using BusinessLogic;
using Microsoft.Extensions.Configuration;


// Set up configuration
AppSettings settings = new();
var config = new ConfigurationBuilder()
    .SetBasePath(AppUtilities.GetCurrentLocationOfTheAppInstallerApp())
    .AddJsonFile("config.json", optional: false, reloadOnChange: true)
    .Build();
config.GetSection("AppSettings").Bind(settings);

CLIInfoController cLIInfo = new(settings);


bool LaunchUI = false;
List<string> UI_arguements = [];

if (args.Length == 0)
{
    LaunchUI = true;
}
else if (args.Length > 0)
{
    foreach (string arg in args)
    {
        string value = arg.Trim();
        if (arg.StartsWith("--"))
        {
            cLIInfo.InterfaceRequest(value);
        }
        else
        {
            UI_arguements.Add(value);
            LaunchUI = true;
        }
    }
}


if (LaunchUI)
{
    Console.WriteLine($"Launching {settings.AppName} UI...");
    Console.WriteLine($"Arguments: {string.Join(", ", UI_arguements)}");

    string arg_text = string.Join(" ", UI_arguements);
    AppUILauncher appUI = new(settings);
    appUI.LaunchAppUI(arg_text);
    cLIInfo.ShowInfo();
    AppUtilities.ExitApp();
}


