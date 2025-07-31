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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppInstaller.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InstallPage : Page
    {
        private Installer installer {  get; set; }
        public InstallPage()
        {
            InitializeComponent();
            installer = new Installer(App.AppConfig);

            this.Loaded += Main_Loaded; //Setting up an event rather than calling the method directly since we need to exit the constructor prior to calling UI updates.
            installer.Progress_Log += UpdateProgress;
        }

        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateTextAsync();
        }

        private void UpdateTextAsync()
        {
            RichEditBoxLogging.AppendColoredText(RichTextBlock, "Starting Application Installation... \n", Color.LightGreen);

            if (App.AppConfig.ValidateThatAllAppNamesMatch() == false)
            {
                RichEditBoxLogging.AppendColoredText(RichTextBlock, "ERROR: The name of the name of the app you are trying to install does not match our expectations.", Color.Red);
                return;
            }

            installer.Run(App.AppConfig.AddVariableToPath);

            RichEditBoxLogging.AppendColoredText(RichTextBlock, "Done! ", Color.LightGreen);
            RichEditBoxLogging.AppendColoredText(RichTextBlock, "This application will close in 5 seconds and relaunch your target application.", Color.White);
            progressBar.Visibility = Visibility.Collapsed;
        }

        private void UpdateProgress(object? sender, string msg)
        {
            Color color_white = Color.White;
            RichEditBoxLogging.AppendColoredText(RichTextBlock, msg, color_white);
            RichEditBoxLogging.AppendColoredText(RichTextBlock, "\n", color_white);
        }
    }
}
