using AppInstaller.Classes;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppInstaller
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public string VersionText { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            NavigateToStartPage();
            SetFixedWindowSize();

            VersionText = GetAppVersion(); // You must set the value before setting the data context
            RootGrid.DataContext = this; // You must do this last
           
        }

        private string GetAppVersion()
        {
            try
            {
                var version = Windows.ApplicationModel.Package.Current.Id.Version;
                return $"Version: {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
            catch
            {
                var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                return "Version: " + version?.ToString() ?? "Unknown";
            }
        }

        private void NavigateToStartPage()
        {
            if(IsAppInstallerInstalled() == false)
            {
                contentFrame.Navigate(typeof(Views.FirstTimeLaunchPage));
                return;
            }
            else if(AppConfigSerivce.GetAppConfig().IsInstallLocationNeeded() == true)
            {
                contentFrame.Navigate(typeof(Views.InstallPromptUser));
                return;
            }
            contentFrame.Navigate(typeof(Views.InstallPage));
        }

        private bool IsAppInstallerInstalled()
        {
            return false;
        }

        private void SetFixedWindowSize()
        {
            IntPtr hwnd = WindowNative.GetWindowHandle(this);
            WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);
            appWindow.SetPresenter(AppWindowPresenterKind.CompactOverlay);
            appWindow.Resize(new Windows.Graphics.SizeInt32(1700, 1300)); // Width, Height
        }
    }
}
