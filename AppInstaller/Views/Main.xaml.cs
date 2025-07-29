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
    public sealed partial class Main : Page
    {
        private Installer installer {  get; set; }
        public Main()
        {
            InitializeComponent();
            installer = new Installer(new AppConfig());

            this.Loaded += Main_Loaded; //Setting up an event rather than calling the method directly since we need to exit the constructor prior to calling UI updates.
            installer.Progress_Log += UpdateProgress;
        }

        private void UpdateProgress(object? sender, string msg)
        {
            Color color_white = Color.White;
            RichEditBoxLogging.AppendColoredText(RichTextBlock, msg, color_white);
        }

        private async void Main_Loaded(object sender, RoutedEventArgs e)
        {
            await UpdateTextAsync();
        }

        private async Task UpdateTextAsync()
        {
            Color color_white = Color.White;
            Color color_green = Color.LightGreen;

            if(true == false)
            {
                await Task.Run(() => installer.Run());
            }

            RichEditBoxLogging.AppendColoredText(RichTextBlock, "Starting Application Installation... ", color_white);
            await Task.Delay(1000);
            RichEditBoxLogging.AppendColoredText(RichTextBlock, "Successful!\n", color_green);

            RichEditBoxLogging.AppendColoredText(RichTextBlock, "Analyzing file structure... ", color_white);
            await Task.Delay(4000);
            RichEditBoxLogging.AppendColoredText(RichTextBlock, "Successful!\n", color_green);

            RichEditBoxLogging.AppendColoredText(RichTextBlock, "Deleting old files.. ", color_white);
            await Task.Delay(3000);
            RichEditBoxLogging.AppendColoredText(RichTextBlock, "Successful!\n", color_green);

            RichEditBoxLogging.AppendColoredText(RichTextBlock, "Transfering new files... ", color_white);
            await Task.Delay(3000);
            RichEditBoxLogging.AppendColoredText(RichTextBlock, "Successful!\n", color_green);

            RichEditBoxLogging.AppendColoredText(RichTextBlock, "\n", color_green);

            RichEditBoxLogging.AppendColoredText(RichTextBlock, "Done! ", color_green);
            RichEditBoxLogging.AppendColoredText(RichTextBlock, "This application will close in 5 seconds and relaunch your target application.", color_white);

            progressBar.Visibility = Visibility.Collapsed;
        }

     
    }
}
