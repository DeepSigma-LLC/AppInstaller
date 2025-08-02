using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace BusinessLogic
{
    public static class EnvironmentVariables
    {
        /// <summary>
        /// Adds new path to a users enviornment variables so that the .exe program is accessible from the terminal.
        /// </summary>
        /// <param name="newPath"></param>
        /// <exception cref="Exception"></exception>
        public static void AddToPath(string newPath)
        {
            const string path_name = "PATH";
            string? currentPath = Environment.GetEnvironmentVariable(path_name, EnvironmentVariableTarget.User);
            if (currentPath is not null && currentPath.Contains(newPath) == false)
            {
                string updatedPath = currentPath + ";" + newPath;
                Environment.SetEnvironmentVariable(path_name, updatedPath, EnvironmentVariableTarget.User);
            }
            else
            {
                throw new Exception("Path variable is already present!");
            }
        }

        public static bool DoesPathExist(string newPath)
        {
            const string path_name = "PATH";
            string? currentPath = Environment.GetEnvironmentVariable(path_name, EnvironmentVariableTarget.User);
            if (currentPath is not null && currentPath.Contains(newPath) == true)
            {
                return true;
            }
            return false;
        }

    }
}
