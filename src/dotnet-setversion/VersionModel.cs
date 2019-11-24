using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet_setversion
{
    public class VersionModel
    {
        public VersionModelDetail Version { get; set; }
        public override string ToString()
        {
            if (Version != null)
            {
                return $"{Version.Major}.{Version.Minor}.{Version.Patch}";
            }

            return "";
        }
    }

    public class VersionModelDetail
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }
    }
}
