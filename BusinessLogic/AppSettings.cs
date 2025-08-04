using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class AppSettings
    {
        public string AppName { get; init; } = String.Empty;
        public string AppDescription { get; init; } = String.Empty;
        public string AppAuthor { get; init; } = String.Empty;
        public string MainDirectory { get; init; } = String.Empty;
        public string CLIDirectory { get; init; } = String.Empty;
        public bool AutoClose { get; set; } = true;
        public AppSettings(){}

        public string GetAppNameWithoutSpaces()
        {
            return AppName.Replace(" ", String.Empty);
        }
    }
}
