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
using Windows.UI;
using System.Threading;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppInstaller.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Main : Page
    {
        public Main()
        {
            InitializeComponent();
            this.Loaded += Main_Loaded;
        }

        private async void Main_Loaded(object sender, RoutedEventArgs e)
        {
            await UpdateLogAsync();
        }

        private async Task UpdateLogAsync()
        {
            AppendColoredText(textbox_log, "Starting Application Installation... ", System.Drawing.Color.White);
            await Task.Delay(1000);
            AppendColoredText(textbox_log, "Successful!\n", System.Drawing.Color.Green);

            AppendColoredText(textbox_log, "Analyzing file structure... ", System.Drawing.Color.White);
            await Task.Delay(4000);
            AppendColoredText(textbox_log, "Successful!\n", System.Drawing.Color.Green);

            AppendColoredText(textbox_log, "Deleting old files.. ", System.Drawing.Color.White);
            await Task.Delay(3000);
            AppendColoredText(textbox_log, "Successful!\n", System.Drawing.Color.Green);

            AppendColoredText(textbox_log, "Transfering new files... ", System.Drawing.Color.White);
            await Task.Delay(3000);
            AppendColoredText(textbox_log, "Successful!\n", System.Drawing.Color.Green);

            AppendColoredText(textbox_log, "\n", System.Drawing.Color.Green);

            AppendColoredText(textbox_log, "Done! ", System.Drawing.Color.Green);
            AppendColoredText(textbox_log, "This application will close in 5 seconds and relaunch your target application.", System.Drawing.Color.White);

            progressBar.Visibility = Visibility.Collapsed;
            
        }

     
        private void AppendColoredText(RichEditBox box, string text, System.Drawing.Color color)
        {
            // Move selection to the end
            box.Document.GetText(Microsoft.UI.Text.TextGetOptions.None, out string existingText);
            int length = existingText.Length;

            // Set selection at end of document
            var doc = box.Document;
            doc.Selection.SetRange(length, length);

            // Insert text
            doc.Selection.Text = text;

            // Apply color format to just-inserted text
            doc.Selection.CharacterFormat.ForegroundColor = ConvertColor(color);
        }

        private Windows.UI.Color ConvertColor(System.Drawing.Color color)
        {
            Windows.UI.Color winUIColor = Windows.UI.Color.FromArgb(
                color.A,
                color.R,
                color.G,
                color.B);
            return winUIColor;
        }


    }
}
