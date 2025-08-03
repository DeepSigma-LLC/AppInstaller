using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;

namespace AppInstaller
{
    public class EntryPoint
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                // Launch the WinUI app as usual
                Microsoft.UI.Xaml.Application.Start((p) => new App());
            }
            catch(Exception ex)
            { 
                Logger.Log("An error occurred while starting the application. Please ensure that the application is installed correctly and try again.", ex);
            }
        }
    }
}
