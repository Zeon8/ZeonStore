using System.Diagnostics;
using System.IO;
using ZeonStore.Common;
using System;

namespace ZeonStore
{
    public class AppRunner
    {
        public static void Run(int appId, string exebutable)
        {
            string id = appId.ToString();
            var path = Path.Combine(Constants.InstalledAppsDirectory, id, exebutable);
            Process.Start(new ProcessStartInfo(path)
            {
                WorkingDirectory = Path.GetDirectoryName(path)
            }); 
        }
    }
}
