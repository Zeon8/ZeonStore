using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeonStore.Common;
using static System.Net.Mime.MediaTypeNames;

namespace ZeonStore
{
    public static class ApplicationInfoExtensions
    {
        public static DownloadFileInfo? GetForCurrentOS(this IEnumerable<DownloadFileInfo> downloads) 
        {
            if (OperatingSystem.IsWindows())
                return downloads.FirstOrDefault(d => d.Platform == "Windows");
            else if (OperatingSystem.IsLinux())
                return downloads.FirstOrDefault(d => d.Platform == "Linux");
            else if (OperatingSystem.IsMacOS())
                return downloads.FirstOrDefault(d => d.Platform == "MacOS");
            return downloads.FirstOrDefault(d => d.Platform == "All");
        }
    }
}
