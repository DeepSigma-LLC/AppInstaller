using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    internal class IgnoreFileController
    {
        private string file_path { get; set; }
        private string file_name { get; set; }
        public IgnoreFileController(string file_path, string file_name)
        {
            this.file_path = file_path;
            this.file_name = file_name;
        }

        /// <summary>
        /// Gets cleaned list of ignore filters from designated file if it exists.
        /// </summary>
        /// <returns></returns>
        internal List<string> GetIgnoreFilters()
        {
            List<string> ignoreFilters = [];
            string FullPath = Path.Combine(file_path, file_name);
            if (File.Exists(FullPath) == false)
            {
                return ignoreFilters;
            }

            string[] fileLines = File.ReadAllLines(FullPath);
            foreach (string line in fileLines)
            {
                string[] words = line.Split(" ");
                foreach (string word in words)
                {
                    if (IgnoreFileUtilities.IsComment(word))
                    {
                        continue;
                    }
                    ignoreFilters.Add(word.Trim());
                }
            }
            return ignoreFilters;
        }


    }
}
