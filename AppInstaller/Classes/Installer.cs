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
        private DirectoryFileUpdateController fileController { get; set; }

        public EventHandler<string>? Progress_Log;
        public Installer(AppConfig config)
        {
            this.config = config;
            this.fileController = new DirectoryFileUpdateController(config.OverwriteConfigFile);

            Progress_Log = fileController.Update_Event_Progress;
        }

        /// <summary>
        /// Runs the file update program. 
        /// </summary>
        public void Run()
        {
            if(String.IsNullOrEmpty(config.AppNameToInstall)) return;

            List<string> filters = config.GetIgnoreFilters();
            fileController.SetFilters(filters);

            Progress_Log?.Invoke(null, "Starting application installation...");
            Progress_Log?.Invoke(null, "Starting directory clean up...");

            string destination_path = Path.Combine(config.TargetInstallLocation, config.AppNameToInstall);
            //fileController.DeleteDirectoryRecursively(destination_path);

            Progress_Log?.Invoke(null, "Directory is now clean...");
            Progress_Log?.Invoke(null, "Initiating file transfer...");

            //fileController.CopyDirectoryRecursively(config.SourceDirectoryPath, destination_path);
            Progress_Log?.Invoke(null, "Installation is now complete!");
        }

      

    }
}
