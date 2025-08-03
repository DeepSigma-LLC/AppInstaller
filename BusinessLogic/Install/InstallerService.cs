using BusinessLogic.Messaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Install
{
    public class InstallerService
    {
        private AppConfig appConfig { get; set; }
        private Installer installer { get; set; }
        private IProgressMessenger? messenger { get; } = null;
        public InstallerService(AppConfig appConfig, IProgressMessenger? messenger = null)
        {
            this.appConfig = appConfig;
            this.messenger = messenger;
            installer = new Installer(appConfig, messenger);
        }

        public async Task RunInstallAsync()
        {
            await (messenger?.PostMessageAsync(new MessageResult("Starting Application Installation... \n", MessageResultType.Success)) ?? Task.CompletedTask);
            if (appConfig.ValidateThatAllAppNamesMatch() == false)
            {
                await (messenger?.PostMessageAsync(new MessageResult("ERROR: The name of the name of the app you are trying to install does not match our expectations.", MessageResultType.Error)) ?? Task.CompletedTask);
                return;
            }

            await (messenger?.PostMessageAsync(new MessageResult("\n\n")) ?? Task.CompletedTask);
            await (messenger?.PostMessageAsync(new MessageResult("///////////////////////////////", MessageResultType.Success)) ?? Task.CompletedTask);
            await (messenger?.PostMessageAsync(new MessageResult("Checking for Main Installation...")) ?? Task.CompletedTask);
            if (appConfig.GetSourceDirectory() is not null)
            {
                await (messenger?.PostMessageAsync(new MessageResult("Starting Main Installation...")) ?? Task.CompletedTask);
                await installer.Run(InstallType.Main, false).ConfigureAwait(false); //Always false.
            }

            await (messenger?.PostMessageAsync(new MessageResult("\n\n")) ?? Task.CompletedTask);
            await (messenger?.PostMessageAsync(new MessageResult("///////////////////////////////", MessageResultType.Success)) ?? Task.CompletedTask);
            await (messenger?.PostMessageAsync(new MessageResult("Checking for CLI Installation...")) ?? Task.CompletedTask);
            if (appConfig.GetSourceCLIDirectory() is not null)
            {
                await (messenger?.PostMessageAsync(new MessageResult("Starting CLI Installation...")) ?? Task.CompletedTask);
                await installer.Run(InstallType.CLI, appConfig.AddVariableToPath).ConfigureAwait(false);
            }

            await (messenger?.PostMessageAsync(new MessageResult("Done! \n", MessageResultType.Success)) ?? Task.CompletedTask);
            await (messenger?.PostMessageAsync(new MessageResult("This application will close in 5 seconds and relaunch your target application.")) ?? Task.CompletedTask);
        }

        public async Task RunInstallAsyncTest()
        {
            for (int i = 0; i < 10; i++)
            {
                await (messenger?.PostMessageAsync(new MessageResult($"Step {i}", MessageResultType.Success)) ?? Task.CompletedTask);
            }
        }
    }
}
