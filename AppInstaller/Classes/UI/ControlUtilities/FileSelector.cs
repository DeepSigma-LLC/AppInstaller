using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace AppInstaller.Classes.UI.ControlUtilities
{
    public class FileSelector : ISystemSelector
    {
        public event EventHandler<string>? ErrorMessagingEvent;
        public async Task<string?> SelectFileAsync(MainWindow window, Button sender, ICollection<string>? filters = null)
        {
            if (filters is null) filters = []; //Initialize to empty list if not set

            sender.IsEnabled = false; //Since async
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            // Initialize the file picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            // Set options for your file picker
            openPicker.ViewMode = PickerViewMode.Thumbnail;

            foreach (var filter in filters)
            {
                openPicker.FileTypeFilter.Add(filter);
            }

            // Open the picker for the user to pick a file
            var file = await openPicker.PickSingleFileAsync();
            string? result = null;
            if (file != null)
            {
                result = file.Path;
            }
            else
            {
                ErrorMessagingEvent?.Invoke(null, "Operation cancelled.");
            }

            //re-enable the button
            sender.IsEnabled = true;
            return result;
        }
    }
}
