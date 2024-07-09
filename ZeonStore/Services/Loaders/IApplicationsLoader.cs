using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeonStore.Common;

namespace ZeonStore.Services.Loaders
{
    public interface IApplicationsLoader
    {
        Task<IEnumerable<ApplicationInfo>> GetApplications(int count, int page);
    }
}
