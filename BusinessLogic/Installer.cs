using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BusinessLogic
{
    internal class Installer
    {
        private AppConfig config { get; set; }
        private DirectoryFileReplacer fileController { get; set; }

        internal EventHandler<MessageResult>? Progress_Log;

        internal Installer(AppConfig config)
        {
            this.config = config;
            this.fileController = new DirectoryFileReplacer();
            Progress_Log = fileController.Update_Event_Progress;
        }

        /// <summary>
        /// Runs the file update program. 
        /// </summary>
        internal void Run(InstallType installType, bool add_to_enivronment_path_variable = false)
        {
            if (String.IsNullOrEmpty(config.AppNameToInstall)) return;

            string? app_name = config.AppNameToInstall;

            Progress_Log?.Invoke(null, new MessageResult("Validating app name..."));
            if (app_name is null)
            {
                Progress_Log?.Invoke(null, new MessageResult("ERROR: App name cannot be null. Ending process...", IsError:true));
                return;
            }

            string? source_directory = GetSourceDirectory(installType);
            Progress_Log?.Invoke(null, new MessageResult("Validating source directory..."));
            if (source_directory is null)
            {
                Progress_Log?.Invoke(null, new MessageResult("ERROR: Source directory cannot be null. Ending process...", IsError:true));
                return;
            }

            Progress_Log?.Invoke(null, new MessageResult("Beginning installation..."));
            Progress_Log?.Invoke(null, new MessageResult("Loading ignore filters..."));
            List<string> filters = config.GetIgnoreFilters();
            fileController.SetFilters(filters);

            Progress_Log?.Invoke(null, new MessageResult("Checking if directory has been created..."));
            string destination_path = GetTargetDirectory(installType, app_name);
            CreateDirectoryIfNeeded(destination_path);

            Progress_Log?.Invoke(null, new MessageResult("Starting directory clean up..."));
            fileController.DeleteDirectoryRecursively(destination_path);

            Progress_Log?.Invoke(null, new MessageResult("Directory is now clean..."));
            Progress_Log?.Invoke(null, new MessageResult("Initiating file transfer..."));

            fileController.CopyDirectoryRecursively(source_directory, destination_path);

            if (add_to_enivronment_path_variable == true)
            {
                Progress_Log?.Invoke(null,  new MessageResult("Adding to user environment path..."));
                EnvironmentVariables.AddToPath(destination_path);
            }

            Progress_Log?.Invoke(null, new MessageResult("Installation is now complete!"));
        }

        private string GetTargetDirectory(InstallType installType, string app_name)
        {
            switch(installType)
            {
                case InstallType.Main:
                    return Path.Combine(config.TargetInstallLocation, app_name, "Main");
                case InstallType.CLI:
                    return Path.Combine(config.TargetInstallLocation, app_name, "CLI");
                default:
                    throw new NotSupportedException("Install type not supported.");
            }
        }

        private string? GetSourceDirectory(InstallType installType)
        {
            switch (installType)
            {
                case InstallType.Main:
                    return config.GetSourceDirectory();
                case InstallType.CLI:
                    return config.GetSourceCLIDirectory();
                default:
                    throw new NotSupportedException("Install type not supported.");
            }
        }

        private void CreateDirectoryIfNeeded(string destination_path)
        {
            if (Directory.Exists(destination_path) == false)
            {
                Progress_Log?.Invoke(null, new MessageResult($"Creating directory: {destination_path}"));
                Directory.CreateDirectory(destination_path);
            }
            else
            {
                Progress_Log?.Invoke(null, new MessageResult($"Directory already exists: {destination_path}"));
            }
        }
    }
}
