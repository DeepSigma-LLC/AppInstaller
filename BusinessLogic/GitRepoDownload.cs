using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class GitRepoDownload
    {
        public GitRepoDownload() { }

        /// <summary>
        /// Runs git clone command and returns the directory info of the resulting download folder where the files are saved.
        /// </summary>
        /// <param name="Git_URL"></param>
        /// <returns></returns>
        public DirectoryInfo RunDownload(string Git_URL)
        {
            DirectoryInfo tempDirectory = TempFolderUtility.CreateTempDirectoryInDownloads(@"GitDownload_");
            string command = $"git clone {Git_URL} \"{tempDirectory}\"";
            string? commandResult = WindowsProcess.ExecuteCommand(command);
            return tempDirectory;
        }
    }
}
