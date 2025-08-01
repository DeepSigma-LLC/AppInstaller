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
            installer.Run(appConfig.AddVariableToPath);

            Progress_Log?.Invoke(null, new MessageResult("Done! "));
            Progress_Log?.Invoke(null, new MessageResult("This application will close in 5 seconds and relaunch your target application."));
        }
    }
}
