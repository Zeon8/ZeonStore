using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeonStore.Common;

namespace ZeonStore.Services
{
    public interface IInstalledAppsRepository
    {
        public IEnumerable<InstalledApplicationInfo> Apps { get; }

        void Load();
        bool Contains(ApplicationInfo application);
        void Add(ApplicationInfo application, Update update);
        void Remove(ApplicationInfo application);
        void SetUpdate(ApplicationInfo application, Update update);
        DateTime GetReleaseDate(ApplicationInfo application);
        string? GetExebutable(ApplicationInfo application);
    }
}
