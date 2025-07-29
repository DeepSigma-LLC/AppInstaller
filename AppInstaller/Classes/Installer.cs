using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppInstaller.Classes
{
    public class Installer
    {
        private AppConfig config { get; set; }
        private DirectoryFileReplacer fileController { get; set; }

        public EventHandler<string>? Progress_Log;
        private bool add_to_enivronment_path_variable { get; }
        public Installer(AppConfig config, bool AddPathVariable = false)
        {
            this.config = config;
            this.fileController = new DirectoryFileReplacer();
            Progress_Log = fileController.Update_Event_Progress;
            add_to_enivronment_path_variable = AddPathVariable;
        }

        /// <summary>
        /// Runs the file update program. 
        /// </summary>
        public void Run()
        {
            if (String.IsNullOrEmpty(config.GetAppNameToInstall())) return;
            Progress_Log?.Invoke(null, "Starting application installation...");
            Progress_Log?.Invoke(null, "Load ignore filters...");
            List<string> filters = config.GetIgnoreFilters();
            fileController.SetFilters(filters);

            string destination_path = Path.Combine(config.TargetInstallLocation, config.GetAppNameToInstall());
            Progress_Log?.Invoke(null, "Checking target directory status...");
            if (ThisIsTheRightDirectory(destination_path) == false)
            {
                Progress_Log?.Invoke(null, "This doesn't look like the right directory. Stopping the update process.");
                return;
            }
            CreateDirectoryIfNeeded(destination_path);

            Progress_Log?.Invoke(null, "Starting directory clean up...");
            fileController.DeleteDirectoryRecursively(destination_path);

            Progress_Log?.Invoke(null, "Directory is now clean...");
            Progress_Log?.Invoke(null, "Initiating file transfer...");

            string source_directory = config.GetSourceDirectory();
            fileController.CopyDirectoryRecursively(source_directory, destination_path);

            if (add_to_enivronment_path_variable == true)
            {
                AddInstalledAppToEnvironmentPath(destination_path);
            }

            Progress_Log?.Invoke(null, "Installation is now complete!");
        }


        private void CreateDirectoryIfNeeded(string destination_path)
        {
            if (Directory.Exists(destination_path) == false)
            {
                Progress_Log?.Invoke(null, $"Creating directory: {destination_path}");
                Directory.CreateDirectory(destination_path);
            }
            else
            {
                Progress_Log?.Invoke(null, $"Directory already exists: {destination_path}");
            }
        }

        private bool ThisIsTheRightDirectory(string app_level_folder_path, bool already_installed = true)
        {
            string destination_path = Path.Combine(config.TargetInstallLocation, config.GetAppNameToInstall());
            if(app_level_folder_path == destination_path)
            {
                return true;
            }
            return false;
        }

        private void AddInstalledAppToEnvironmentPath(string installed_app_directory_path)
        {
            string app_file_full_path = Path.Combine(installed_app_directory_path, App.AppConfig.AppExeFile);
            if (add_to_enivronment_path_variable == true)
            {
                EnvironmentVariables.AddToPath(app_file_full_path);
            }
        }
    }
}
