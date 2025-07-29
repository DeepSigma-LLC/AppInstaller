using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace AppInstaller.Classes
{
    public static class EnvironmentVariables
    {
       public static void AddToPath(string newPath)
       {
            const string path_name = "PATH";
            string? currentPath = Environment.GetEnvironmentVariable(path_name, EnvironmentVariableTarget.User);
            if (currentPath is not null && currentPath.Contains(newPath) == false)
            {
                string updatedPath = currentPath + ";" + newPath;
                Environment.SetEnvironmentVariable(path_name, updatedPath);
            }
            else
            {
                throw new Exception("Path variable is already present!");
            }
       }

    }
}
