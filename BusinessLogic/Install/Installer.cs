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

            await (messenger?.PostMessageAsync(new MessageResult("Validating app name...")) ?? Task.CompletedTask);
            if (app_name is null)
            {
                await (messenger?.PostMessageAsync(new MessageResult("ERROR: App name cannot be null. Ending process...", MessageResultType.Error)) ?? Task.CompletedTask);
                return;
            }

            string? source_directory = GetSourceDirectory(installType);
            await (messenger?.PostMessageAsync(new MessageResult("Validating source directory...")) ?? Task.CompletedTask);
            if (source_directory is null)
            {
                await (messenger?.PostMessageAsync(new MessageResult("ERROR: Source directory cannot be null. Ending process...", MessageResultType.Error)) ?? Task.CompletedTask);
                return;
            }

            await (messenger?.PostMessageAsync(new MessageResult("Beginning installation...")) ?? Task.CompletedTask);
            await (messenger?.PostMessageAsync(new MessageResult("Loading ignore filters...")) ?? Task.CompletedTask);
            List<string> filters = config.GetIgnoreFilters();
            fileController.SetFilters(filters);

            await (messenger?.PostMessageAsync(new MessageResult("Checking if directory has been created...")) ?? Task.CompletedTask);
            string destination_path = GetTargetDirectory(installType, app_name);
            await CreateDirectoryIfNeededAsync(destination_path);

            await (messenger?.PostMessageAsync(new MessageResult("Starting directory clean up...")) ?? Task.CompletedTask);
            await fileController.DeleteDirectoryRecursively(destination_path);

            await (messenger?.PostMessageAsync(new MessageResult("Directory is now clean...")) ?? Task.CompletedTask);
            await (messenger?.PostMessageAsync(new MessageResult("Initiating file transfer...")) ?? Task.CompletedTask);
            await fileController.CopyDirectoryRecursively(source_directory, destination_path);

            if (EnvironmentVariables.DoesPathExist(destination_path) == false && add_to_enivronment_path_variable == true)
            {
                await (messenger?.PostMessageAsync(new MessageResult("Adding to user environment path...")) ?? Task.CompletedTask);
                await Task.Run(() => EnvironmentVariables.AddToPath(destination_path)).ConfigureAwait(false);
            }

            await (messenger?.PostMessageAsync(new MessageResult("Installation is now complete!", MessageResultType.Success)) ?? Task.CompletedTask);
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

        private async Task CreateDirectoryIfNeededAsync(string destination_path)
        {
            if (Directory.Exists(destination_path) == false)
            {
                await (messenger?.PostMessageAsync(new MessageResult($"Creating directory: {destination_path}")) ?? Task.CompletedTask);
                Directory.CreateDirectory(destination_path);
            }
            else
            {
                await (messenger?.PostMessageAsync(new MessageResult($"Directory already exists: {destination_path}")) ?? Task.CompletedTask);
            }
        }
    }
}
