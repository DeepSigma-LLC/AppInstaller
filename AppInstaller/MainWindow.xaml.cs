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
using BusinessLogic;

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
        public string AppName => App.AppSettings.AppName;
        public MainWindow()
        {
            InitializeComponent();
            NavigateToStartPage();
            SetFixedWindowSize();

            VersionText = AppUtilities.GetAppVersion(); // You must set the value before setting the data context
            RootGrid.DataContext = this; // You must do this last
        }

        private void NavigateToStartPage()
        {
            if(IsAppInstallerInstalled() == false)
            {
                contentFrame.Navigate(typeof(Views.FirstTimeLaunchPage));
                return;
            }
            else if(App.InstallConfig.OkToAutoInstall())
            {
                contentFrame.Navigate(typeof(Views.InstallPage));
                return;
            }
            contentFrame.Navigate(typeof(Views.InstallPromptUser));
        }

        private bool IsAppInstallerInstalled()
        {

            return BusinessLogic.WindowsProcess.IsProgramInstalled(App.AppSettings.GetAppNameWithoutSpaces());
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
