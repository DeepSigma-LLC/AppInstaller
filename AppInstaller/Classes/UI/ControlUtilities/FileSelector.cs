using AppInstallerUI.Classes;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace AppInstaller.Classes.UI.ControlUtilities
{
    public class FileSelector : ISystemSelector
    {
        public event EventHandler<string>? ErrorMessagingEvent;

        /// <summary>
        /// Enables users to select a file using the windows file system explorer.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="sender"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public async Task<string?> SelectFileAsync(MainWindow window, Button sender, ICollection<string>? filters = null)
        {
            if (filters is null) filters = ["*"]; //Initialize to empty list if not set

            await window.DispatcherQueue.EnqueueAsync(() =>
            {
                sender.IsEnabled = false;
            });
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            openPicker.ViewMode = PickerViewMode.Thumbnail;

            foreach (var filter in filters)
            {
                openPicker.FileTypeFilter.Add(filter);
            }

            // Open the picker for the user to pick a file
            StorageFile file = await openPicker.PickSingleFileAsync();
            string? result = null;
            if (file != null)
            {
                result = file.Path;
            }
            else
            {
                ErrorMessagingEvent?.Invoke(null, "Operation cancelled.");
            }

            await window.DispatcherQueue.EnqueueAsync(() =>
            {
                sender.IsEnabled = true; //re-enable the button
            });
            return result;
        }
    }
}
