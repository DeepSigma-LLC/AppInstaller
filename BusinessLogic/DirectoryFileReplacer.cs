using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class DirectoryFileReplacer
    {
        public EventHandler<MessageResult>? Update_Event_Progress;
        private List<string> filters { get; set; }
        public DirectoryFileReplacer(List<string>? filters = null)
        {
            this.filters = filters ?? [];
        }

        /// <summary>
        /// Set filters to be used during the file update.
        /// </summary>
        /// <param name="filters"></param>
        public void SetFilters(List<string> filters)
        {
            this.filters = filters;
        }

        /// <summary>
        /// Recursively copies all items of a directory to a new directroy.
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="destinationDir"></param>
        /// <param name="overwrite"></param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public void CopyDirectoryRecursively(string source_directory, string destinationDir)
        {
            if (Directory.Exists(source_directory) == false)
            {
                throw new DirectoryNotFoundException($"Source directory does not exist: {source_directory}");
            }

            if (Directory.Exists(destinationDir) == false)
            {
                Directory.CreateDirectory(destinationDir);
            }

            CopyAllFiles(source_directory, destinationDir);
            CopyEachSubDirectoryRecursively(source_directory, destinationDir);
        }

        /// <summary>
        /// Recursively deletes all items of a directory.
        /// </summary>
        /// <param name="directory_path"></param>
        /// <param name="filters"></param>
        public void DeleteDirectoryRecursively(string directory_path)
        {
            if (Directory.Exists(directory_path) == false)
            {
                return;
            }

            DeleteFiles(directory_path);
            DeleteEachSubDirectoryRecursively(directory_path);
        }

        //Loop through every sub directroy in the source directory and copy all files and sub directories.
        private void CopyEachSubDirectoryRecursively(string source_directory, string destinationDir)
        {
            foreach (string subDir in Directory.GetDirectories(source_directory))
            {
                string dirName = Path.GetFileName(subDir);
                string destSubDir = Path.Combine(destinationDir, dirName);
                CopyDirectoryRecursively(subDir, destSubDir);
                Update_Event_Progress?.Invoke(null, new MessageResult("Directory copied: " + dirName));
            }
        }

        /// <summary>
        /// Copies all files from a directroy to a the destination directory.
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="destinationDir"></param>
        private void CopyAllFiles(string sourceDir, string destinationDir)
        {
            foreach (string filePath in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(filePath);
                string destFilePath = Path.Combine(destinationDir, fileName);
                File.Copy(filePath, destFilePath);
                Update_Event_Progress?.Invoke(null, new MessageResult("File copied: " + fileName));
            }
        }


        /// <summary>
        /// Loops through each sub directory recursively and deletes all files and directories. 
        /// </summary>
        /// <param name="directory_path"></param>
        private void DeleteEachSubDirectoryRecursively(string directory_path)
        {
            foreach (string subDir in Directory.GetDirectories(directory_path))
            {
                string dirName = Path.GetFileName(subDir);
                string destSubDir = Path.Combine(directory_path, dirName);
                DeleteDirectoryRecursively(destSubDir);
                DeleteDirectories(destSubDir);
                Update_Event_Progress?.Invoke(null, new MessageResult("Directory delected: " + dirName));
            }
        }


        /// <summary>
        /// Deletes files in directory if no matching exclusion filter is found.
        /// </summary>
        /// <param name="current_install_path"></param>
        /// <param name="filters"></param>
        private void DeleteFiles(string current_install_path)
        {
            foreach (string file in Directory.GetFiles(current_install_path))
            {
                if (OkToDelete(file))
                {
                    File.Delete(Path.Combine(current_install_path, file));
                    Update_Event_Progress?.Invoke(null, new MessageResult("File deleted: " + file));
                }
                else
                {
                    Update_Event_Progress?.Invoke(null, new MessageResult("File Ignored: " + file));
                }
            }
        }

        /// <summary>
        /// Deletes directories if no matching exclusion filter is found.
        /// </summary>
        /// <param name="current_install_path"></param>
        /// <param name="filters"></param>
        private void DeleteDirectories(string current_install_path)
        {
            foreach (string directory in Directory.GetDirectories(current_install_path))
            {
                if (OkToDelete(directory))
                {
                    File.Delete(Path.Combine(current_install_path, directory));
                    Update_Event_Progress?.Invoke(null, new MessageResult("Directory deleted: " + directory));
                }
                else
                {
                    Update_Event_Progress?.Invoke(null, new MessageResult("Directory Ignored: " + directory));
                }
            }
        }

        /// <summary>
        /// It is ok to delete a item if there is no matching filter.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        private bool OkToDelete(string file_system_item)
        {
            return !IgnoreFileUtilities.IgnoreItem(file_system_item, filters);
        }

    }
}
