// See https://aka.ms/new-console-template for more information
// Program.cs
using AppInstallerCLI;
using BusinessLogic;
using System.Diagnostics.SymbolStore;

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
            CLIInfoController.InterfaceRequest(value);
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
    Console.WriteLine("Launching App Installer UI...");
    Console.WriteLine($"Arguements: {string.Join(", ", UI_arguements)}");
    string arg_text = string.Join(" ", UI_arguements);
    AppUILauncher.LaunchAppUI(arg_text);
    CLIInfoController.ShowInfo();
    AppUtilities.ExitApp();
}


