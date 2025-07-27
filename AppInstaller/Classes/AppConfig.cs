using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AppInstaller.Classes
{
    internal class AppConfig
    {
        internal bool UserSelectsInstallLocation { get; set; } = false;
        public AppConfig() { }
    }
}
