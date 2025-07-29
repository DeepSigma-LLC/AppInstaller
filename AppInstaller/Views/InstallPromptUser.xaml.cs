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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppInstaller.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InstallPromptUser : Page
    {

        private FolderSelector fileSelector {  get; set; }
        private string? selectedInstallLocation { get; set; }
        public InstallPromptUser()
        {
            InitializeComponent();
            fileSelector = new FolderSelector();
            SetGitVersion();
        }

        private void Git_Repo_Download_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void Install_Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsInstallPathValid())
            {
                await MessageBox.ShowDialogAsync((FrameworkElement)sender,
                contentMessage: "Your install is about to begin. \n\nAre you sure you want to continue?",
                primaryButtonText: "Yes",
                successMethod: Install,
                closeButtonText: "Cancel");
                return;
            }
            await MessageBox.ShowDialogAsync((FrameworkElement)sender,
                contentMessage: "Invalid install path. \n\nPlease select a valid and empty installation path.",
                primaryButtonText: "Ok");
        }

        private bool IsInstallPathValid()
        {
            string selectedPath = selectedFilePath.Text;
            bool isPathValid = Directory.Exists(selectedPath);

            bool isPathEmpty = false;
            if (isPathValid)
            {
                isPathEmpty = Directory.EnumerateFileSystemEntries(selectedPath).Count() == 0;
            }
            return isPathValid && isPathEmpty;
        }

        private void Install()
        {
            this.Frame.Navigate(typeof(InstallPage));
        }

        private async void Select_Folder_Click(object sender, RoutedEventArgs e)
        {
            selectedFilePath.Text = String.Empty;
            if(App.MyWindow is not null)
            {
                selectedInstallLocation = await fileSelector.SelectFolderAsync(App.MyWindow, (Button)sender);
                selectedFilePath.Text = selectedInstallLocation ?? String.Empty;
            }
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            AppUtilities.ExitApp();
        }

        private async void SetGitVersion()
        {
            string? result = await WindowsProcess.ProgramInstalledVersion("git");
            textbox_Installed_Git_Version.Text = result ?? "Not Installed";
        }
    }
}
