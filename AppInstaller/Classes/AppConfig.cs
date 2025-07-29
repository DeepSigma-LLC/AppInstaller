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
        public string AppExeFile { get; set; } = String.Empty;
        public string IgnoreFileName { get; set; } = "AppIgnore.txt";
        public string SourceDirectoryPath { private get; set; } = String.Empty;
        public string TargetInstallLocation { get; set; } = String.Empty;
        public bool IsInstallLocationNeeded()
        {
            if(TargetInstallLocation is null || TargetInstallLocation == String.Empty)
            {
                return true;
            }
            return false;
        }

        public string GetCurrentLocationOfTheAppInstallerApp()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public bool DoesSourceDirectoryContainVersioningDirectories()
        {
            string app_file_path = Path.Combine(SourceDirectoryPath, AppExeFile);
            string? max_version = TryGetLatestVersionDirectory();
            if(String.IsNullOrEmpty(max_version) == false)
            {
                return true;
            }
            return false;
        }

        public string GetSourceDirectory()
        {
            string source_directory = SourceDirectoryPath;
            string? LatestVersionDirectory = TryGetLatestVersionDirectory();
            if (DoesSourceDirectoryContainVersioningDirectories() == true && LatestVersionDirectory is not null)
            {
                source_directory = Path.Combine(source_directory, LatestVersionDirectory);
            }
            return source_directory;
        }

       
        public List<string> GetIgnoreFilters()
        {
            string selectedPath = TargetInstallLocation ?? GetCurrentLocationOfTheAppInstallerApp();
            IgnoreFileController controller = new IgnoreFileController(selectedPath, IgnoreFileName);
            return controller.GetIgnoreFilters();
        }

        public string GetAppNameToInstall()
        {
            return this.AppExeFile.Replace(".exe", "");
        }
        private string? TryGetLatestVersionDirectory()
        {
            return AppVersioningService.GetLatestVersionFolder(SourceDirectoryPath);
        }
    }
}
