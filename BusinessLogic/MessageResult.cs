using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class MessageResult
    {
        public string? Message { get; set; }
        public ErrorEventArgs? ErrorEventArgs { get; set; }
        public string? ErrorStackTrace { get; set; }
        public bool IsError { get; set; } = false;
        public MessageResult() { }
        public MessageResult(string? Message, bool? IsError = null, ErrorEventArgs? e = null)
        {
            this.Message = Message;

            if(IsError is not null) 
            {
                IsError = IsError.Value;
            }

            if(e != null)
            {
                this.ErrorEventArgs = e;
                this.ErrorStackTrace = e?.GetException()?.StackTrace ?? string.Empty;
                this.IsError = true;
            }
        }
    }
}
