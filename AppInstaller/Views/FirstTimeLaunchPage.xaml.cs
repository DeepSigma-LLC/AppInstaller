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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using AppInstaller.Classes;

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
            string install_folder = SelectedInstallLocation.Text.Trim();
            if (Directory.Exists(install_folder) == false)
            {
                await MessageBox.ShowDialogAsync(this, "Invalid Install Location Selected.", "OK");
                return;
            }
            
            App.AppConfig.TargetInstallLocation = install_folder;
            this.Frame.Navigate(typeof(InstallPage));
        }

        private async void Select_Folder_Location_Button_Click(object sender, RoutedEventArgs e)
        {
            if(App.MyWindow is not null)
            {
                string? selectedFolder = await folderSelector.SelectFolderAsync(App.MyWindow, (Button)sender);
                SelectedInstallLocation.Text = selectedFolder ?? String.Empty;
            }
        }
    }
}
