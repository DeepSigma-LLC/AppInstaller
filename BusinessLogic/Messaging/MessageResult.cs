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
        public string? Message { get; init; }
        public ErrorEventArgs? ErrorEventArgs { get; init; }
        public string? ErrorStackTrace { get; init; }
        public MessageResultType MessageResultType { get; init; } = MessageResultType.Default;
        public DateTime EntryDateTime { get; init; } = DateTime.Now;
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
