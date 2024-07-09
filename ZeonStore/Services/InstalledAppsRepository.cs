using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ZeonStore.Common;

namespace ZeonStore.Services
{
    public class InstalledAppsRepository : IInstalledAppsRepository
    {
        public IEnumerable<InstalledApplicationInfo> Apps => _apps;

        private List<InstalledApplicationInfo> _apps = new();

        private const string FileName = "apps.json";

        public void Load()
        {
            if (File.Exists(FileName))
            {
                var json = File.ReadAllText(FileName);
                var apps = JsonSerializer.Deserialize<List<InstalledApplicationInfo>>(json);
                if (apps is not null)
                    _apps = apps;
            }
        }

        public void Add(ApplicationInfo application, Update update)
        {
            var exebutable = update.InstallFiles.GetForCurrentOS();
            _apps.Add(new InstalledApplicationInfo(application.Id, update.Version, update.ReleaseDate, exebutable?.ExebutableName));
            Save();
        }

        public void Remove(ApplicationInfo application)
        {
            var installed = _apps.First(a => a.Id == application.Id);
            _apps.Remove(installed);
            Save();
        }

        public void SetUpdate(ApplicationInfo application, Update update)
        {
            InstalledApplicationInfo installed = _apps.First(a => a.Id == application.Id);
            installed.Version = update.Version;
            installed.ReleaseDate = update.ReleaseDate;

            var exebutable = update.UpdateFiles.GetForCurrentOS();
            if(exebutable is not null)
                installed.ExebutableFile = exebutable.ExebutableName;

            Save();
        }

        public DateTime GetReleaseDate(ApplicationInfo application)
        {
            return _apps.First(a => a.Id == application.Id).ReleaseDate;
        }

        public string? GetExebutable(ApplicationInfo application)
        {
            return _apps.First(a => a.Id == application.Id).ExebutableFile;
        }

        private void Save()
        {
            var json = JsonSerializer.Serialize(_apps);
            File.WriteAllText(FileName, json);
        }

        public bool Contains(ApplicationInfo application)
        {
            return _apps.Any(a => a.Id == application.Id);
        }
    }
}
