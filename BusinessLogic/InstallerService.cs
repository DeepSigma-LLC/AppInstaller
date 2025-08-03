using BusinessLogic.Messaging;
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
        private IProgressMessenger? messenger = null;
        public InstallerService(AppConfig appConfig, IProgressMessenger? messenger = null)
        {
            this.appConfig = appConfig;
            this.messenger = messenger;
            this.installer = new Installer(appConfig, messenger);
        }

        public async Task RunInstall()
        {
            messenger?.PostMessage(new MessageResult("Starting Application Installation... \n", MessageResultType.Success));

            if (appConfig.ValidateThatAllAppNamesMatch() == false)
            {
                messenger?.PostMessage(new MessageResult("ERROR: The name of the name of the app you are trying to install does not match our expectations.", MessageResultType.Error));
                return;
            }

            messenger?.PostMessage(new MessageResult("\n\n"));
            messenger?.PostMessage(new MessageResult("///////////////////////////////", MessageResultType.Success));
            messenger?.PostMessage(new MessageResult("Checking for Main Installation..."));
            if (appConfig.GetSourceDirectory() is not null)
            {
                messenger?.PostMessage(new MessageResult("Starting Main Installation..."));
                await installer.Run(InstallType.Main, false); //Always false.
            }

            messenger?.PostMessage(new MessageResult("\n\n"));
            messenger?.PostMessage(new MessageResult("///////////////////////////////", MessageResultType.Success));
            messenger?.PostMessage(new MessageResult("Checking for CLI Installation..."));
            if (appConfig.GetSourceCLIDirectory() is not null)
            {
                messenger?.PostMessage(new MessageResult("Starting CLI Installation..."));
                await installer.Run(InstallType.CLI, appConfig.AddVariableToPath);
            }

            messenger?.PostMessage(new MessageResult("Done! \n", MessageResultType.Success));
            messenger?.PostMessage(new MessageResult("This application will close in 5 seconds and relaunch your target application."));
        }

    }
}
