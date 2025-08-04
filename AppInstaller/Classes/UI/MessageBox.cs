using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppInstallerUI.Classes.UI
{
    internal static class MessageBox
    {
        public static async Task ShowDialogAsync(FrameworkElement contextElement, string contentMessage, string primaryButtonText, Action? successMethod = null, string? secondaryButtonText = null, Action? secondaryMethod = null, string? closeButtonText = null, string title = "")
        {
            var dialog = new ContentDialog
            {
                XamlRoot = contextElement.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = title,
                Content = new TextBlock { Text = contentMessage, TextWrapping=TextWrapping.WrapWholeWords},
                PrimaryButtonText = primaryButtonText,
                SecondaryButtonText = secondaryButtonText,
                CloseButtonText = closeButtonText,
            };

            var result = await dialog.ShowAsync();
            switch (result)
            {
                case ContentDialogResult.Primary:
                    successMethod?.Invoke();
                    break;
                case ContentDialogResult.Secondary:
                    secondaryMethod?.Invoke();
                    break;
            }
        }
    }
}
