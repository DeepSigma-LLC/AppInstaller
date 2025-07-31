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

        public Installer(AppConfig config)
        {
            this.config = config;
            this.fileController = new DirectoryFileReplacer();
            Progress_Log = fileController.Update_Event_Progress;
        }

        /// <summary>
        /// Runs the file update program. 
        /// </summary>
        public void Run(bool add_to_enivronment_path_variable = false)
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

            Progress_Log?.Invoke(null, "Beginning installation...");
            Progress_Log?.Invoke(null, "Loading ignore filters...");
            List<string> filters = config.GetIgnoreFilters();
            fileController.SetFilters(filters);

            Progress_Log?.Invoke(null, "Checking if directory has been created...");
            string destination_path = Path.Combine(config.TargetInstallLocation, app_name);
            CreateDirectoryIfNeeded(destination_path);

            Progress_Log?.Invoke(null, "Starting directory clean up...");
            fileController.DeleteDirectoryRecursively(destination_path);

            Progress_Log?.Invoke(null, "Directory is now clean...");
            Progress_Log?.Invoke(null, "Initiating file transfer...");

            fileController.CopyDirectoryRecursively(source_directory, destination_path);

            if (add_to_enivronment_path_variable == true)
            {
                Progress_Log?.Invoke(null, "Adding to user environment path...");
                EnvironmentVariables.AddToPath(destination_path);
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
    }
}
