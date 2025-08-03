using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Messaging
{
    public class MessageResult
    {
        public string? Message { get; set; }
        public ErrorEventArgs? ErrorEventArgs { get; set; }
        public string? ErrorStackTrace { get; set; }
        public MessageResultType MessageResultType { get; set; } = MessageResultType.Default;
        public MessageResult() { }
        public MessageResult(string? Message, MessageResultType messageResultType = MessageResultType.Default, ErrorEventArgs? e = null)
        {
            this.Message = Message;
            MessageResultType = messageResultType;

            if (e != null)
            {
                ErrorEventArgs = e;
                ErrorStackTrace = e?.GetException()?.StackTrace ?? string.Empty;
                MessageResultType = MessageResultType.Error;
            }
        }
    }
}
