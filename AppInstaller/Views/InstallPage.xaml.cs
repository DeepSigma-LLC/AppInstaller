using AppInstallerUI.Classes;
using BusinessLogic;
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
        private EventHandler? EndApp;
        public InstallPage()
        {
            InitializeComponent();
            dispatcher = DispatcherQueue.GetForCurrentThread();
            IProgressMessenger messenger = new DispatcherProgressMessenger(dispatcher, UpdateProgress);
            installer = new InstallerService(App.AppConfig, messenger);

            this.Loaded += Main_Loaded; //Setting up an event rather than calling the method directly since we need to exit the constructor prior to calling UI updates.
            EndApp += Kill;
        }

        private async void Main_Loaded(object sender, RoutedEventArgs e)
        {
            await UpdateTextAsync();

            await dispatcher.EnqueueAsync(() =>
            {
                progressBar.Visibility = Visibility.Collapsed;
            });
            EndApp?.Invoke(this, EventArgs.Empty); 
        }

        private void Kill(object? sender, EventArgs e)
        {
            Thread.Sleep(5000); // Allow time for the UI to update before closing
            AppUtilities.ExitApp();
        }

        private async Task UpdateTextAsync()
        {
            await installer.RunInstall();
        }

        private async void UpdateProgress(object? sender, MessageResult msg)
        {
            Color selectedcolor = msg.MessageResultType switch
            {
                MessageResultType.Success => Color.LightGreen,
                MessageResultType.Warning => Color.Yellow,
                MessageResultType.Error => Color.Red,
                _ => Color.White
            };

            await dispatcher.EnqueueAsync(() =>
            {
                RichEditBoxLogging.AppendColoredText(LogBox, msg.Message ?? string.Empty, selectedcolor);
                RichEditBoxLogging.AppendColoredText(LogBox, "\n", Color.White);

                LogBox.Document.Selection.SetRange(LogBox.Document.Selection.EndPosition, LogBox.Document.Selection.EndPosition);
                LogBox.Focus(FocusState.Programmatic);
            });
            await Task.Yield(); // Yield to allow UI to update before continuing
        }

    }
}
