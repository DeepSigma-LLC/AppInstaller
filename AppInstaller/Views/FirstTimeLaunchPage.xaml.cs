using AppInstaller.Classes;
using AppInstaller.Classes.UI.ControlUtilities;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using BusinessLogic;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppInstaller.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FirstTimeLaunchPage : Page
    {
        private FolderSelector folderSelector { get; set; } = new();
        private string recommend_install_path { get; set; } = @"C:\Users\" + Environment.UserName + @"\AppData\Local\";
        public FirstTimeLaunchPage()
        {
            InitializeComponent();
            SelectedInstallLocation.Text = recommend_install_path;
        }

        private async void Install_Button_Click(object sender, RoutedEventArgs e)
        {

            string cli_directory = SelectedCLILocation.Text.Trim();
            string install_folder = SelectedInstallLocation.Text.Trim();
            bool valid_directories = await ValidateDirectories(cli_directory, install_folder);
            if (!valid_directories) {return;}

            App.AppConfig.SourceCLIDirectoryPath = cli_directory;
            App.AppConfig.SourceDirectoryPath = AppUtilities.GetCurrentLocationOfTheAppInstallerApp();
            App.AppConfig.TargetInstallLocation = install_folder;
            App.AppConfig.AppNameToInstall = App.NameWithoutSpaces;
            App.AppConfig.AddVariableToPath = checkbox_AddPathVariable.IsChecked ?? false;
            this.Frame.Navigate(typeof(InstallPage));
        }

        private async System.Threading.Tasks.Task<bool> ValidateDirectories(string cli_directory, string install_folder)
        {
            if (Directory.Exists(cli_directory) == false)
            {
                await MessageBox.ShowDialogAsync(this, "Invalid CLI Folder Location Selected.", "OK");
                return false;
            }

            if (Directory.Exists(install_folder) == false)
            {
                await MessageBox.ShowDialogAsync(this, "Invalid Install Location Selected.", "OK");
                return false;
            }

            return true;
        }

        private async void Select_Folder_CLI_Location_Button_Click(object sender, RoutedEventArgs e)
        {
            if (App.MyWindow is not null)
            {
                string? selectedFolder = await folderSelector.SelectFolderAsync(App.MyWindow, (Button)sender);
                SelectedCLILocation.Text = selectedFolder ?? String.Empty;
            }
        }

        private async void Select_Folder_Location_Button_Click(object sender, RoutedEventArgs e)
        {
            if(App.MyWindow is not null)
            {
                string? selectedFolder = await folderSelector.SelectFolderAsync(App.MyWindow, (Button)sender);
                SelectedInstallLocation.Text = selectedFolder ?? String.Empty;
            }
        }

        private void Skip_Button_Click(object sender, RoutedEventArgs args)
        {
            this.Frame.Navigate(typeof(InstallPromptUser));
        }
    }
}
