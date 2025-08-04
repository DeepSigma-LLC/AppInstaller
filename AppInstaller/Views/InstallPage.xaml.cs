using AppInstallerUI.Classes;
using BusinessLogic;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Dispatching;
using BusinessLogic.Messaging;
using BusinessLogic.Install;
using AppInstallerUI.Classes.UI;
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
        private DispatcherQueue dispatcher { get; set; }
        private MessageResultCollection MessageResults {get; set; } = new();

        public InstallPage()
        {
            InitializeComponent();
            dispatcher = DispatcherQueue.GetForCurrentThread();
            IProgressMessenger messenger = new DispatcherProgressMessenger(dispatcher, UpdateProgress);
            installer = new InstallerService(App.InstallConfig, App.AppSettings, messenger);

            this.Loaded += Main_Loaded; //Setting up an event rather than calling the method directly since we need to exit the constructor prior to calling UI updates.
        }

        private async void Main_Loaded(object sender, RoutedEventArgs e)
        {
            // make sure the bar is visible while installing
            progressBar.Visibility = Visibility.Visible;
            button_cancel.IsEnabled = false;
            button_export.IsEnabled = false;

            await Task.Run(async () => await installer.RunInstallAsync()); // Must use Task.Run since the installer is doing IO operations.

            // back on the UI thread: hide the bar
            await dispatcher.EnqueueAsync(() =>
            {
                button_cancel.IsEnabled = true;
                button_export.IsEnabled = true;
                progressBar.Visibility = Visibility.Collapsed;
            });

            await Task.Delay(5000); // Wait for 5 seconds to allow the user to read the results before closing the app.

            if (App.AppSettings.AutoClose)
            {
                AppUtilities.ExitApp();
            }
            else
            {
                await dispatcher.EnqueueAsync(async() =>
                {
                    await MessageBox.ShowDialogAsync(this, "Installation complete! The application will not close automatically since auto-close is disabled. You can now use the installed application.", "OK");

                });
            }
        }

        private async void UpdateProgress(object? sender, MessageResult msg)
        {
            MessageResults.Add(msg);
            await dispatcher.EnqueueAsync(() =>
            {
                Color selectedcolor = msg.MessageResultType switch
                {
                    MessageResultType.Success => Color.LightGreen,
                    MessageResultType.Warning => Color.Yellow,
                    MessageResultType.Error => Color.Red,
                    _ => Color.White
                };

                RichEditBoxLogging.AppendColoredText(LogBox, msg.Message ?? string.Empty, selectedcolor);
                RichEditBoxLogging.AppendColoredText(LogBox, "\n", Color.White);

                // Scroll to the end of the log box
                LogBox.Document.Selection.SetRange(LogBox.Document.Selection.EndPosition, LogBox.Document.Selection.EndPosition);
                LogBox.Focus(FocusState.Programmatic);
            });
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
           App.AppSettings.AutoClose = false; // Disable auto-close so the user can see the results of the install.
        }

        private async void Button_Export_Click(object sender, RoutedEventArgs e)
        {
            string[] messages = MessageResults.GetDataForLogFile();
            string? download_path = TempFolderUtility.GetDownloadsPath();
            if (download_path is null)
            {
                await MessageBox.ShowDialogAsync(this, "Unable to export log file. Downloads folder not found.", "Error");
                return;
            }
            string file_path = Path.Combine(download_path, $"InstallLog-{Guid.NewGuid()}.txt");
            File.WriteAllLines(file_path, messages);
            WindowsProcess.ExecuteExeFileDirectly(file_path, string.Empty);
        }
    }
}
