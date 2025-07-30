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

            string? app_name = config.GetAppNameToInstall();
            Progress_Log?.Invoke(null, "Validating app name...");
            if (app_name is null)
            {
                Progress_Log?.Invoke(null, "ERROR: App name cannot be null. Ending process...");
                return;
            }

            string? source_directory = config.GetSourceDirectory();
            Progress_Log?.Invoke(null, "Validating source directory...");
            if (source_directory is null)
            {
                Progress_Log?.Invoke(null, "ERROR: Source directory cannot be null. Ending process...");
                return;
            }

            string destination_path = Path.Combine(config.TargetInstallLocation, app_name);
            Progress_Log?.Invoke(null, "Validating target directory...");
            if (ThisIsTheRightDirectory(destination_path, app_name) == false)
            {
                Progress_Log?.Invoke(null, "This doesn't look like the right directory. Stopping the update process.");
                return;
            }

            Progress_Log?.Invoke(null, "Starting application installation...");
            Progress_Log?.Invoke(null, "Load ignore filters...");
            List<string> filters = config.GetIgnoreFilters();
            fileController.SetFilters(filters);

            CreateDirectoryIfNeeded(destination_path);

            Progress_Log?.Invoke(null, "Starting directory clean up...");
            fileController.DeleteDirectoryRecursively(destination_path);

            Progress_Log?.Invoke(null, "Directory is now clean...");
            Progress_Log?.Invoke(null, "Initiating file transfer...");

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

        private bool ThisIsTheRightDirectory(string app_level_folder_path, string app_name, bool already_installed = true)
        {
            string destination_path = Path.Combine(config.TargetInstallLocation, app_name);
            if(app_level_folder_path == destination_path)
            {
                return true;
            }
            return false;
        }

        private void AddInstalledAppToEnvironmentPath(string installed_app_directory_path)
        {
            if (add_to_enivronment_path_variable == true)
            {
                EnvironmentVariables.AddToPath(installed_app_directory_path);
            }
        }
    }
}
