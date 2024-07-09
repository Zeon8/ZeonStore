using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeonStore.Common;

namespace ZeonStore.Services.Loaders
{
    public class DefaultLoader : IApplicationsLoader
    {
        private readonly ApplicationsService _service;

        public DefaultLoader(ApplicationsService service)
        {
            _service = service;
        }

        public Task<IEnumerable<ApplicationInfo>> GetApplications(int count, int page)
        {
            return _service.GetApplications(count, page);
        }
    }
}
