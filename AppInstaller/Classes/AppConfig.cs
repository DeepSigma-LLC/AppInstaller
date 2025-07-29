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
    public class AppConfig
    {
        public string AppNameToInstall { get; set; } = String.Empty;
        public string IgnoreFileName { get; set; } = "AppIgnore.txt";
        public string SourceDirectoryPath { get; set; } = String.Empty;
        public bool DoesSourceDirectoryContainVersioningDirectories { get; set; } = false;
        public string TargetInstallLocation { get; set; } = String.Empty;
        public string? NetworkDeploymentFolderPath { get; set; } = null;
        public bool OverwriteConfigFile { get; set; } = false;
        public bool IsInstallLocationNeeded()
        {
            if(TargetInstallLocation is null || TargetInstallLocation == String.Empty)
            {
                return true;
            }
            return false;
        }

        public bool InstallFromNetworkFolder()
        {
            if(String.IsNullOrEmpty(NetworkDeploymentFolderPath) == false || Directory.Exists(NetworkDeploymentFolderPath))
            {
                return true;
            }
            return false;
        }

        public string GetCurrentLocationOfTheAppInstallerApp()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public List<string> GetIgnoreFilters()
        {
            string selectedPath = Assembly.GetExecutingAssembly().Location;
            if (InstallFromNetworkFolder())
            {
                selectedPath = NetworkDeploymentFolderPath ?? selectedPath;
            }
            IgnoreFileController controller = new IgnoreFileController(selectedPath, IgnoreFileName);
            return controller.GetIgnoreFilters();
        }
    }
}
