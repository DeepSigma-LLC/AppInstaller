using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppInstaller.Classes
{
    internal static class IgnoreFileUtilities
    {


        /// <summary>
        /// Returns true if the file object passes matches a filter criteria.
        /// </summary>
        /// <param name="file_system_item"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        internal static bool IgnoreItem(string file_system_item, List<string> filters)
        {
            foreach (string filter in filters)
            {
                string search_text = IgnoreFileUtilities.GetFilterText(filter);
                var result = IgnoreFileUtilities.GetFilterInfo(filter);

                if (result.StartsWithAnything && result.EndsWithAnything)
                {
                    if (file_system_item.Contains(search_text)) return true;
                }
                else if (result.StartsWithAnything)
                {
                    if (file_system_item.StartsWith(search_text)) return true;
                }
                else if (result.EndsWithAnything)
                {
                    if (file_system_item.EndsWith(search_text)) return true;
                }
            }
            return false;
        }

        //Removes special characters from the filter string.
        internal static string GetFilterText(string filter)
        {
            return filter.Replace("\\", null).Replace("*", null);
        }

        /// <summary>
        /// Gets the ignore filter info.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>
        /// </returns>
        internal static (bool IsDirectory, bool StartsWithAnything, bool EndsWithAnything) GetFilterInfo(string filter)
        {
            bool IsDirectory = false;
            bool StartsWithAnything = false;
            bool EndsWithAnything = false;

            if (filter.StartsWith("\\"))
            {
                IsDirectory = true;
            }

            if (filter.StartsWith("\\*") || filter.StartsWith("*"))
            {
                StartsWithAnything = true;
            }

            if (filter.EndsWith("*"))
            {
                EndsWithAnything = true;
            }
            return (IsDirectory, StartsWithAnything, EndsWithAnything);
        }

        /// <summary>
        /// Determines if text is a comment as I define it.
        /// Comments start with # and last the entire line.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        internal static bool IsComment(string text)
        {
            return text.Trim().StartsWith("#");
        }
    }
}
