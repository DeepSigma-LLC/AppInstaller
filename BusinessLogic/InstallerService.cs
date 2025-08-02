using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class InstallerService
    {
        private AppConfig appConfig { get; set; }
        private Installer installer { get; set; }

        public EventHandler<MessageResult>? Progress_Log;
        public InstallerService(AppConfig appConfig)
        {
            this.appConfig = appConfig;
            this.installer = new Installer(appConfig);
        }

        public void RunInstall()
        {
            Progress_Log?.Invoke(null, new MessageResult("Starting Application Installation... \n"));

            if (appConfig.ValidateThatAllAppNamesMatch() == false)
            {
                Progress_Log?.Invoke(null, new MessageResult("ERROR: The name of the name of the app you are trying to install does not match our expectations.", IsError:true));
                return;
            }

            Progress_Log?.Invoke(null, new MessageResult("Checking for Main Installation... \n"));
            if (appConfig.GetSourceDirectory() is null)
            {
                Progress_Log?.Invoke(null, new MessageResult("Starting Main Installation... \n"));
                installer.Run(InstallType.Main, false); //Always false.
            }

            Progress_Log?.Invoke(null, new MessageResult("Checking for CLI Installation... \n"));
            if (appConfig.GetSourceCLIDirectory() is not null)
            {
                Progress_Log?.Invoke(null, new MessageResult("Starting CLI Installation... \n"));
                installer.Run(InstallType.CLI, appConfig.AddVariableToPath);
            }
           
            Progress_Log?.Invoke(null, new MessageResult("Done! "));
            Progress_Log?.Invoke(null, new MessageResult("This application will close in 5 seconds and relaunch your target application."));
        }

    }
}
