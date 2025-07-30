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


        public string? GetSourceDirectory()
        {
            string? source_directory = SourceDirectoryPath;
            if(String.IsNullOrEmpty(source_directory)) return null;

            string? LatestVersionDirectory = TryGetLatestVersionDirectory();
            if (LatestVersionDirectory is not null && DoesSourceDirectoryContainVersioningDirectories() == true)
            {
                source_directory = Path.Combine(source_directory, LatestVersionDirectory);
            }
            return source_directory;
        }

        public bool DoesSourceDirectoryContainVersioningDirectories()
        {
            string? max_version = TryGetLatestVersionDirectory();
            return !String.IsNullOrEmpty(max_version);
        }

        public List<string> GetIgnoreFilters()
        {
            string selectedPath = TargetInstallLocation ?? GetCurrentLocationOfTheAppInstallerApp();
            IgnoreFileController controller = new IgnoreFileController(selectedPath, IgnoreFileName);
            return controller.GetIgnoreFilters();
        }

        public string? GetAppExeFileNameToInstall()
        {
            string? SourceDirectory = GetSourceDirectory();
            if(SourceDirectory is null) return null;

            string[] files = Directory.GetFiles(SourceDirectory);
            foreach(string file in files)
            {
                if (file.EndsWith(".exe"))
                {
                    return Path.GetFileName(file);
                }
            }
            return null;
        }

        public string? GetAppNameToInstall()
        {
            string? file_name = GetAppExeFileNameToInstall();
            if(file_name is null) return null;
            return file_name.Replace(".exe", "");
        }
        private string? TryGetLatestVersionDirectory()
        {
            return AppVersioningService.GetLatestVersionFolder(SourceDirectoryPath);
        }
    }
}
