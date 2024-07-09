using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeonStore.Common;

namespace ZeonStore.Services
{
    public interface IApplicationsService
    {
        Task<IEnumerable<ApplicationInfo>> GetApplications();
        Task<ApplicationDetailedInfo?> GetDetailedInfo(ApplicationInfo application);
        Task<ApplicationDetailedInfo?> GetDetailedInfo(int applicationId);

        Task<Update?> GetLatestUpdate(ApplicationInfo app);
        Task<IEnumerable<Update>?> GetUpdates(ApplicationInfo app, DateTime releaseDate);

    }
}
