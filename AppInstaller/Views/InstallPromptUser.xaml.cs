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
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppInstaller.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InstallPromptUser : Page
    {
        public InstallPromptUser()
        {
            InitializeComponent();
        }

        private void Install_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Main));
        }

        private async void Select_Folder_Click(object sender, RoutedEventArgs e)
        {
            selectedFilePath.Text = String.Empty;

            var senderButton = sender as Button;
            if (senderButton is null) return;
            senderButton.IsEnabled = false;

            var openPicker = new Windows.Storage.Pickers.FolderPicker();
            var window = App.MyWindow;

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            // Initialize the file picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            // Set options for your file picker
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add("*");

            // Open the picker for the user to pick a file
            var folder = await openPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                selectedFilePath.Text = folder.Path;
            }

            senderButton.IsEnabled = true;
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
        }
    }
}
