using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public static class AppUtilities
    {
        public static void ExitApp()
        {
            Environment.Exit(1);
        }

        /// <summary>
        /// Returns the current directory of this application.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentLocationOfTheAppInstallerApp()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public static string GetAppVersion()
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            return "Version: " + version?.ToString() ?? "Unknown";
        }
    }
}
