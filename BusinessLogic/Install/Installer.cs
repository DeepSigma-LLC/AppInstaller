using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using BusinessLogic.Messaging;

namespace BusinessLogic.Install
{
    internal class Installer
    {
        private AppConfig config { get; set; }
        private DirectoryFileReplacer fileController { get; set; }

        private IProgressMessenger? messenger { get; set; }
        internal Installer(AppConfig config, IProgressMessenger? messenger = null)
        {
            this.config = config;
            this.messenger = messenger;
            fileController = new DirectoryFileReplacer(messenger);
        }

        /// <summary>
        /// Runs the file update program. 
        /// </summary>
        internal async Task Run(InstallType installType, bool add_to_enivronment_path_variable = false)
        {
            if (string.IsNullOrEmpty(config.AppNameToInstall)) return;

            string? app_name = config.AppNameToInstall;

            messenger?.PostMessage(new MessageResult("Validating app name..."));
            if (app_name is null)
            {
                messenger?.PostMessage(new MessageResult("ERROR: App name cannot be null. Ending process...", MessageResultType.Error));
                return;
            }

            string? source_directory = GetSourceDirectory(installType);
            messenger?.PostMessage(new MessageResult("Validating source directory..."));
            if (source_directory is null)
            {
                messenger?.PostMessage(new MessageResult("ERROR: Source directory cannot be null. Ending process...", MessageResultType.Error));
                return;
            }

            messenger?.PostMessage(new MessageResult("Beginning installation..."));
            messenger?.PostMessage(new MessageResult("Loading ignore filters..."));
            List<string> filters = config.GetIgnoreFilters();
            fileController.SetFilters(filters);

            messenger?.PostMessage(new MessageResult("Checking if directory has been created..."));
            string destination_path = GetTargetDirectory(installType, app_name);
            await Task.Run(() => CreateDirectoryIfNeeded(destination_path));

            messenger?.PostMessage(new MessageResult("Starting directory clean up..."));
            await Task.Run(() => fileController.DeleteDirectoryRecursively(destination_path));

            messenger?.PostMessage(new MessageResult("Directory is now clean..."));
            messenger?.PostMessage(new MessageResult("Initiating file transfer..."));

            await Task.Run(() => fileController.CopyDirectoryRecursively(source_directory, destination_path));

            if (EnvironmentVariables.DoesPathExist(destination_path) == false && add_to_enivronment_path_variable == true)
            {
                messenger?.PostMessage(new MessageResult("Adding to user environment path..."));
                await Task.Run(() => EnvironmentVariables.AddToPath(destination_path));
            }

            messenger?.PostMessage(new MessageResult("Installation is now complete!", MessageResultType.Success));
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
                messenger?.PostMessage(new MessageResult($"Creating directory: {destination_path}"));
                Directory.CreateDirectory(destination_path);
            }
            else
            {
                messenger?.PostMessage(new MessageResult($"Directory already exists: {destination_path}"));
            }
        }
    }
}
