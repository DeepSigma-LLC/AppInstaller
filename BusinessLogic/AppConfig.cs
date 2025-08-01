using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class AppConfig
    { 
        public string IgnoreFileName { get;} = "AppIgnore.txt";
        public string AppNameUsedForValidation { get; set; } = string.Empty;
        public string SourceDirectoryPath { private get; set; } = string.Empty;
        public string TargetInstallLocation { get; set; } = string.Empty;
        public bool AddVariableToPath { get; set; } = false;


        /// <summary>
        /// Gets the raw app name from the .exe file with the extension removed from the source directory.
        /// </summary>
        /// <returns></returns>
        public string? GetAppNameToInstall()
        {
            string? file_name = GetAppExeFileNameToInstall();
            if (file_name is null) return null;
            return file_name.Replace(".exe", "");
        }

        /// <summary>
        /// Gets the application .exe file name with extension still attached from the source directory.
        /// </summary>
        /// <returns></returns>
        public string? GetAppExeFileNameToInstall()
        {
            string? SourceDirectory = GetSourceDirectory();
            if (SourceDirectory is null) return null;
            return GetExeFileNameFromLocation(SourceDirectory);
        }

        /// <summary>
        /// Determines if the target install location is needed.
        /// </summary>
        /// <returns></returns>
        public bool IsInstallLocationNeeded()
        {
            return string.IsNullOrEmpty(TargetInstallLocation);
        }

        /// <summary>
        /// Validates that the app your are trying to install has a valid name and matches the app we are about to install.
        /// Additionally, if we are performing an app update we also validate that we are re-installing the same app.
        /// </summary>
        /// <returns></returns>
        public bool ValidateThatAllAppNamesMatch()
        {
            if (string.IsNullOrEmpty(AppNameUsedForValidation))
            {
                return false;
            }
            bool NewAppNameIsValid = GetAppNameToInstall() == AppNameUsedForValidation;

            bool OddAppNameIdValid = true;
            if (IsAppUpdate())
            {
                OddAppNameIdValid = GetAppExeFileNameAlreadyInstalled() == AppNameUsedForValidation;
            }
            return NewAppNameIsValid && OddAppNameIdValid;
        }

        /// <summary>
        /// Determines if we are performing a new install or updating an existing app.
        /// </summary>
        /// <returns></returns>
        public bool IsAppUpdate()
        {
            if (Directory.Exists(TargetInstallLocation) == false) return false;

            string? file_name = GetExeFileNameFromLocation(TargetInstallLocation);
            return !string.IsNullOrEmpty(file_name);
        }

        /// <summary>
        /// Gets the application file source directory and is able to handle directories that contain versioned sub directories.
        /// For example, if the source directory is a folder containing versions of the application in folders using the format "#_#_#_#" 
        /// this function will return the path to the directory with the greatest version iteration.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Determins if the source directory contains versioning sub directories.
        /// </summary>
        /// <returns></returns>
        private bool DoesSourceDirectoryContainVersioningDirectories()
        {
            string? max_version = TryGetLatestVersionDirectory();
            return !String.IsNullOrEmpty(max_version);
        }

        /// <summary>
        /// Gets the ignore filters from the AppIgnore.txt file in either the app install location or the source directory in that order.
        /// </summary>
        /// <returns></returns>
        public List<string> GetIgnoreFilters()
        {
            string? selectedPath = TargetInstallLocation ?? GetSourceDirectory();
            if (selectedPath is null) return [];

            IgnoreFileController controller = new IgnoreFileController(selectedPath, IgnoreFileName);
            return controller.GetIgnoreFilters();
        }

        /// <summary>
        /// Returns the application .exe file that is already installed on your PC if it exists in the target directoy location.
        /// </summary>
        /// <returns></returns>
        private string? GetAppExeFileNameAlreadyInstalled()
        {
            if (TargetInstallLocation is null) return null;
            return GetExeFileNameFromLocation(TargetInstallLocation);
        }

        /// <summary>
        /// Gets the first .exe file name from a selected directory path. 
        /// The method only looks in the current directory. It does not recursively look inward.
        /// </summary>
        /// <param name="directory_path"></param>
        /// <returns></returns>
        private string? GetExeFileNameFromLocation(string directory_path)
        {
            if (Directory.Exists(directory_path) == false) return null;

            foreach (string file in Directory.GetFiles(directory_path))
            {
                if (file.EndsWith(".exe"))
                {
                    return Path.GetFileName(file);
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the latest version sub directory name from the designated source directory.
        /// </summary>
        /// <returns></returns>
        private string? TryGetLatestVersionDirectory()
        {
            return AppVersioningService.GetLatestVersionFolder(SourceDirectoryPath);
        }
    }
}
