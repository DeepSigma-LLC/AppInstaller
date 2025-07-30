using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppInstaller.Classes
{
    internal static class AppVersioningService
    {
        
        /// <summary>
        /// Returns the latest app version from the production app deplyment folder, if it exists.
        /// </summary>
        /// <param name="ProductionDeploymentDirectory"></param>
        /// <returns></returns>
        internal static string? GetLatestVersionFolder(string ProductionDeploymentDirectory)
        {
            if (Path.Exists(ProductionDeploymentDirectory) == false) return null;


            List<AppVersion> versions = [];
            string[] DirectoryNames = Directory.GetDirectories(ProductionDeploymentDirectory);
            foreach (string DirectoryName in DirectoryNames)
            {
                AppVersion? version = GetVersion(DirectoryName);
                if (version is not null)
                {
                    versions.Add(version);
                }
            }
            if (versions.Count == 0) return null;

            int Major = versions.Select(x => x.Major).Max();
            int Minor = versions.Where(x => x.Major == Major).Select(x => x.Minor).Max();
            int Build = versions.Where(x => x.Major == Major && x.Minor == Minor).Select(x => x.Build).Max();
            int Patch = versions.Where(x => x.Major == Major && x.Minor == Minor && x.Build == Build).Select(x => x.Patch).Max();

            return Major + "_" + Minor + "_" + Build + "_" + Patch;
        }

        /// <summary>
        /// Returns the custom app version object if it can parse the values from the directory name.
        /// </summary>
        /// <param name="DirectoryName"></param>
        /// <returns></returns>
        private static AppVersion? GetVersion(string DirectoryName)
        {
            string[] values = DirectoryName.Split('_');
            if(values.Length != 4) return null;

            bool MajorSuccess = Int32.TryParse(values[0], out int Major);
            bool MinorSuccess = Int32.TryParse(values[1], out int Minor);
            bool BuildSuccess = Int32.TryParse(values[2], out int Build);
            bool PatchSuccess = Int32.TryParse(values[3], out int Patch);

            if (MajorSuccess && MinorSuccess && BuildSuccess && PatchSuccess)
            {
                return new AppVersion(Major, Minor, Build, Patch);
            }
            return null;
        }
    }
}
