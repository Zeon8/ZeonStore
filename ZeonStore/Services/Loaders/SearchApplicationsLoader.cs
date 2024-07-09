using System.Collections.Generic;
using System.Threading.Tasks;
using ZeonStore.Common;

namespace ZeonStore.Services.Loaders
{
    public class SearchApplicationsLoader : IApplicationsLoader
    {
        private readonly ApplicationsService _applicationsService;

        public SearchApplicationsLoader(ApplicationsService applicationsService)
        {
            _applicationsService = applicationsService;
        }

        public string ApplicationName { get; set; } = string.Empty;

        public Task<IEnumerable<ApplicationInfo>> GetApplications(int count, int page)
        {
            return _applicationsService.Search(ApplicationName, count, page);
        }
    }
}
