using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading;
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
    public sealed partial class InstallPage : Page
    {
        private InstallerService installer {  get; set; }
        public InstallPage()
        {
            InitializeComponent();
            installer = new InstallerService(App.AppConfig);

            this.Loaded += Main_Loaded; //Setting up an event rather than calling the method directly since we need to exit the constructor prior to calling UI updates.
            installer.Progress_Log += UpdateProgress;
        }

        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateTextAsync();
            progressBar.Visibility = Visibility.Collapsed;
        }

        private void UpdateTextAsync()
        {
            installer.RunInstall();
        }

        private void UpdateProgress(object? sender, MessageResult msg)
        {
            if(msg.IsError)
            {
                RichEditBoxLogging.AppendColoredText(LogBox, msg.Message ?? string.Empty, Color.Red);
            }
            else
            {
                RichEditBoxLogging.AppendColoredText(LogBox, msg.Message ?? string.Empty, Color.White);
                RichEditBoxLogging.AppendColoredText(LogBox, "\n", Color.White);
            }
        }
    }
}
