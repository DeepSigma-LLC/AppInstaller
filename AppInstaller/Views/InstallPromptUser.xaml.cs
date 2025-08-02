using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using AppInstaller.Classes.UI.ControlUtilities;
using AppInstaller.Classes;
using BusinessLogic;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppInstaller.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InstallPromptUser : Page
    {
        private FolderSelector fileSelector { get; set; } = new();
        private string? selectedInstallLocation { get; set; }
        public InstallPromptUser()
        {
            InitializeComponent();
            SetGitVersion();
            LoadValuesFromAppConfig();
        }

        private void LoadValuesFromAppConfig()
        {
            textbox_source_directory.Text = App.AppConfig.GetSourceDirectory() ?? String.Empty;
            textbox_selectedFilePath.Text = App.AppConfig.TargetInstallLocation;
            textbox_appName.Text = App.AppConfig.AppNameToInstall;
        }

        private void Git_Repo_Download_Click(object sender, RoutedEventArgs e)
        {
            GitRepoDownload repoDownload = new GitRepoDownload();
            DirectoryInfo results = repoDownload.RunDownload(SelectedGitUrl.Text.Trim());
            WindowsProcess.OpenFolder(results.FullName);
            LoadValuesFromAppConfig();
        }

        private async void Install_Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsInstallPathValid())
            {
                await MessageBox.ShowDialogAsync((FrameworkElement)sender,
                contentMessage: "Your install is about to begin. \n\nAre you sure you want to continue?",
                primaryButtonText: "Yes",
                successMethod: GoToInstall,
                closeButtonText: "Cancel");
                return;
            }
            await MessageBox.ShowDialogAsync((FrameworkElement)sender,
                contentMessage: "Invalid install path. \n\nPlease select a valid and empty installation path.",
                primaryButtonText: "Ok");
        }

        private bool IsInstallPathValid()
        {
            string selectedPath = App.AppConfig.TargetInstallLocation;
            bool isPathValid = Directory.Exists(selectedPath);

            if (isPathValid == false)
            {
                return false;
            }
            else if (App.AppConfig.IsAppUpdate())
            {
                return isPathValid;
            }
            bool isPathEmpty = Directory.EnumerateFileSystemEntries(selectedPath).Count() == 0;
            return isPathValid && isPathEmpty;
           
        }

        private void GoToInstall()
        {
            this.Frame.Navigate(typeof(InstallPage));
        }

        private async void Select_Folder_Click(object sender, RoutedEventArgs e)
        {
            if(App.MyWindow is not null)
            {
                selectedInstallLocation = await fileSelector.SelectFolderAsync(App.MyWindow, (Button)sender);
                App.AppConfig.TargetInstallLocation = selectedInstallLocation ?? String.Empty;
            }
            LoadValuesFromAppConfig();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            AppUtilities.ExitApp();
        }

        private void SetGitVersion()
        {
            string? result = WindowsProcess.ProgramInstalledVersion("git");
            textbox_Installed_Git_Version.Text = result ?? "Not Installed";
        }
    }
}
