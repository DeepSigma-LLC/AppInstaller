﻿using AppInstaller.Classes;
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
        public static AppConfig AppConfig { get; set; } = new();
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
            MyWindow = new MainWindow();
            MyWindow.Activate();
            StoreInstalledAppPathPassedAsArgument(args);
        }

        private void StoreInstalledAppPathPassedAsArgument(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {

            string[] commandArgs = Environment.GetCommandLineArgs();

            if (commandArgs.Length >= 1)
            {
                AppConfig.AppNameToInstall = commandArgs[0];
            }
            
            if(commandArgs.Length >= 2)
            {
                AppConfig.SourceDirectoryPath = commandArgs[1];
            }

            if (commandArgs.Length >= 3)
            {
                AppConfig.TargetInstallLocation = commandArgs[2];
            }

            if (commandArgs.Length >= 4)
            {
                AppConfig.SourceCLIDirectoryPath = commandArgs[3];
            }
        }

    }
}
