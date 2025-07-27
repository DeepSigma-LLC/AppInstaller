using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AppInstaller.Classes
{
    internal class AppConfig
    {
        internal string TargetInstallLocation { get; set; } = String.Empty;
        internal string AppNameToInstall { get; set; } = String.Empty;
        internal string? NetworkDeploymentFolderPath { get; set; } = null;

        internal bool IsInstallLocationNeeded()
        {
            if(TargetInstallLocation is null || TargetInstallLocation == String.Empty)
            {
                return true;
            }
            return false;
        }

        internal bool InstallFromNetworkFolder()
        {
            if(NetworkDeploymentFolderPath is not null || Directory.Exists(NetworkDeploymentFolderPath))
            {
                return true;
            }
            return false;
        }

        internal string GetCurrentLocationOfTheAppInstallerApp()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
