using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppInstaller.Classes.UI.ControlUtilities
{
    internal interface ISystemSelector
    {
        event EventHandler<string>? ErrorMessagingEvent;
    }
}
