using AppInstaller.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AppInstaller
{
    public class EntryPoint
    {

        [STAThread]
        public static void Main(string[] args)
        {
           if (args.Length >= 1 && args[0].Trim().ToLower() == "--version")
           {
                Environment.Exit(0);
            }
            else
            {
                // Launch the WinUI app as usual
                Microsoft.UI.Xaml.Application.Start((p) => new App());
            }
        }
    }
}
