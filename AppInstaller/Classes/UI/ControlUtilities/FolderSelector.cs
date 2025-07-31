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
    internal class FolderSelector : ISystemSelector
    {
        public event EventHandler<string>? ErrorMessagingEvent;

        /// <summary>
        /// Enables users to select a file using the windows folder system explorer.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="sender"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public async Task<string?> SelectFolderAsync(MainWindow window, Button sender, ICollection<string>? filters = null)
        {
            if (filters is null) filters = ["*"]; //Initialize to empty list if not set

            sender.IsEnabled = false; // since async
            var openPicker = new Windows.Storage.Pickers.FolderPicker();
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            // Set options for your file picker
            openPicker.ViewMode = PickerViewMode.Thumbnail;

            foreach (var filter in filters)
            {
                openPicker.FileTypeFilter.Add(filter);
            }

            // Open the picker for the user to pick a file
            StorageFolder folder = await openPicker.PickSingleFolderAsync();
            string? result = null;
            if (folder != null)
            {
                result = folder.Path;
            }
            else
            {
                ErrorMessagingEvent?.Invoke(null, "Operation cancelled.");
            }

            //reenable the button
            sender.IsEnabled = true;
            return result;
        }

    }
}
