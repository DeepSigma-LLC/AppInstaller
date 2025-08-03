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
        string value = arg.Trim().ToLower();
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
    Console.WriteLine($"Arguements: {string.Join(",", UI_arguements)}");
    string? UI_arg1 = UI_arguements.Count >= 1 ? UI_arguements[0] : null;
    string? UI_arg2 = UI_arguements.Count >= 2 ? UI_arguements[1] : null;
    string? UI_arg3 = UI_arguements.Count >= 3 ? UI_arguements[2] : null;
    AppUILauncher.LaunchAppUI(UI_arg1, UI_arg2, UI_arg3);
    CLIInfoController.ShowInfo();
    AppUtilities.ExitApp();
}


