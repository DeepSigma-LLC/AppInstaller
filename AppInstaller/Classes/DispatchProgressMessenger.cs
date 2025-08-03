using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;
using BusinessLogic.Messaging;

namespace AppInstallerUI.Classes
{
    public class DispatcherProgressMessenger : IProgressMessenger
    {
        private readonly DispatcherQueue _dispatcher;
        private readonly EventHandler<MessageResult> _handler;

        public DispatcherProgressMessenger(DispatcherQueue dispatcher, EventHandler<MessageResult> handler)
        {
            _dispatcher = dispatcher;
            _handler = handler;
        }

        public void PostMessage(MessageResult message)
        {
            _dispatcher.TryEnqueue(() => _handler?.Invoke(this, message));
        }
    }
}
