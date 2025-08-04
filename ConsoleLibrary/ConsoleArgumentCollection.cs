using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLibrary
{
    public class ConsoleArgumentCollection
    {
        private SortedDictionary<string, ConsoleArgument> _arguments = new();
        public ConsoleArgumentCollection()
        {
            
        }

        public void Add(string argumentName, ConsoleArgument consoleArgument)
        {
            if (string.IsNullOrWhiteSpace(argumentName))
            {
                throw new ArgumentException("Argument name cannot be null or whitespace.", nameof(argumentName));
            }
            if (consoleArgument == null)
            {
                throw new ArgumentNullException(nameof(consoleArgument), "Argument value cannot be null.");
            }
            _arguments[argumentName] = consoleArgument;
        }

        public SortedDictionary<string, ConsoleArgument> GetCollection()
        {
            return _arguments;
        }
    }
}
