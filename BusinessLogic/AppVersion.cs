using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    internal class AppVersion
    {
        internal int Major {  get; set; }
        internal int Minor { get; set; }
        internal int Build { get; set; }
        internal int Patch { get; set; }

        public AppVersion(int Major, int Minor, int Build, int Patch)
        {
            this.Major = Major;
            this.Minor = Minor;
            this.Build = Build;
            this.Patch = Patch;
        }
    }
}
