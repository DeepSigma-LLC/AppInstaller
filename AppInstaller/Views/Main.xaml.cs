using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
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
            UpdateLog();
        }

        private void UpdateLog()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Starting Application Installation...\n");
            sb.Append("Analzing structure...\n");
            sb.Append("Updating Application Files...\n");
            sb.Append("Application Succesfully Updated!\n");

            textbox_log.Text = sb.ToString();
        }
    }
}
