using BusinessLogic;
using BusinessLogic.Install;
using Microsoft.Extensions.Configuration;
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
        public static InstallConfig InstallConfig { get; } = new();
        public static MainWindow? MyWindow { get; private set; }
        public static AppSettings AppSettings { get; } = new();
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            // Set up configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(AppUtilities.GetCurrentLocationOfTheAppInstallerApp())
                .AddJsonFile("config.json", optional: false, reloadOnChange: true)
                .Build();

            config.GetSection("AppSettings").Bind(AppSettings);

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
                    InstallConfig.AppNameToInstall = value;
                    break;
                case "source":
                    InstallConfig.SourceDirectoryPath = value;
                    break;
                case "target":
                    InstallConfig.TargetInstallLocation = value;
                    break;
                case "clisource":
                    InstallConfig.SourceCLIDirectoryPath = value;
                    break;
                case "auto":
                    bool successfullyParsed = bool.TryParse(value, out bool result);
                    if (successfullyParsed) {InstallConfig.AutoInstall = result; }
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
