using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLibrary
{
    public class ConsoleArgumentCollection
    {
        private SortedDictionary<string, ConsoleArgument> Arguments = new();
        public ConsoleArgumentCollection(){}

        public void Add(string argumentName, ConsoleArgument consoleArgument)
        {
            ValidateArgumentName(argumentName);
            ValidateConsoleArgument(consoleArgument);
            Arguments[argumentName] = consoleArgument;
        }

        public SortedDictionary<string, ConsoleArgument> GetCollection()
        {
            return Arguments;
        }

        private void ValidateArgumentName(string argumentName)
        {
            if (string.IsNullOrWhiteSpace(argumentName))
            {
                throw new ArgumentException("Argument name cannot be null or whitespace.", nameof(argumentName));
            }
        }

        private void ValidateConsoleArgument(ConsoleArgument argument)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(nameof(argument), "Argument value cannot be null.");
            }
        }
    }
}
