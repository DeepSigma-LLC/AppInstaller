using AppInstaller.Classes;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.Windows.AppLifecycle;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using BusinessLogic;
using Windows.Foundation.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppInstaller
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static string Name { get; } = "App Installer";
        public static string NameWithoutSpaces { get; } = Name.Replace(" ", String.Empty);
        public static AppConfig AppConfig { get; } = new();
        public static MainWindow? MyWindow { get; private set; }
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            StoreInstalledAppPathPassedAsArgument(args); //Must be called before creating the window to ensure values are set correctly for later steps.

            MyWindow = new MainWindow();
            MyWindow.Activate();
        }

        private void StoreInstalledAppPathPassedAsArgument(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            string[] commandArgs = Environment.GetCommandLineArgs();

            int startIndex = 0;
            foreach (var argument in commandArgs)
            {
                SetValueFromArgument(argument);
                startIndex++;
            }
        }

        private void SetValueFromArgument(string argument)
        {
            (string key, string value) = GetKeyValuePairFromArgument(argument);
            switch (key.ToLower())
            {
                case "app":
                    AppConfig.AppNameToInstall = value;
                    break;
                case "source":
                    AppConfig.SourceDirectoryPath = value;
                    break;
                case "target":
                    AppConfig.TargetInstallLocation = value;
                    break;
                case "clisource":
                    AppConfig.SourceCLIDirectoryPath = value;
                    break;
                case "auto":
                    bool successfullyParsed = bool.TryParse(value, out bool result);
                    if (successfullyParsed) {AppConfig.AutoInstall = result; }
                    break;
                default:
                    break;
            }
        }

        private (string, string) GetKeyValuePairFromArgument(string argument)
        {
            string[] inputs = argument.Split("=", StringSplitOptions.TrimEntries);
            (string key, string value) = (inputs.Length > 0 ? inputs[0] : string.Empty, inputs.Length > 1 ? inputs[1] : string.Empty);
            return (key, value);
        }
    }
}
