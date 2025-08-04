using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLibrary
{
    public class ConsoleArgument
    {
        public string Description { get; set; } = String.Empty;
        public Action Method { get; set; }

        public ConsoleArgument(Action action, string description)
        {
            this.Description = description;
            this.Method = action;
        }
    }
}
