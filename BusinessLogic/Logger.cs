using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public static class Logger
    {
        public static void Log(string message, Exception? e)
        {
            string[] content = new string[]
            {
                $"{DateTime.Now.ToString()} : {message}",
                $"Stack Trace: {e?.ToString()}"
            };
            string? download_path = TempFolderUtility.GetDownloadsPath();
            if (download_path is null) { return;}

            string logFilePath = System.IO.Path.Combine(download_path, $"AppInstallerLog-{Guid.NewGuid()}.txt");
            File.WriteAllLines(logFilePath, content);    
        }
    }
}
