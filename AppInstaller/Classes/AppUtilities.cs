using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppInstaller.Classes
{
    internal static class AppUtilities
    {
        internal static void ExitApp()
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
            try
            {
                var version = Windows.ApplicationModel.Package.Current.Id.Version;
                return $"Version: {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
            catch
            {
                var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                return "Version: " + version?.ToString() ?? "Unknown";
            }
        }

    }
}
