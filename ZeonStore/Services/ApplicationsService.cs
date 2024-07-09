using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using ZeonStore.Common;

namespace ZeonStore.Services
{
    public class ApplicationsService : IApplicationsService
    {
        private readonly HttpClient _client = new()
        {
            BaseAddress = new Uri(Constants.WebApiUrl)
        };

        public async Task<IEnumerable<ApplicationInfo>> GetApplications()
        {
            var apps = await _client.GetFromJsonAsync<IEnumerable<ApplicationInfo>>("Applications/");
            return apps ?? [];
        }

        public async Task<IEnumerable<ApplicationInfo>> GetApplications(int count, int page)
        {
            var apps = await _client.GetFromJsonAsync<IEnumerable<ApplicationInfo>>($"Applications/Limited?count={count}&page={page}");
            return apps ?? [];
        }

        public Task<ApplicationDetailedInfo?> GetDetailedInfo(ApplicationInfo app)
        {
            return GetDetailedInfo(app.Id);
        }

        public Task<ApplicationDetailedInfo?> GetDetailedInfo(int applicationId)
        {
            string url = $"Applications/{applicationId}";
            return _client.GetFromJsonAsync<ApplicationDetailedInfo>(url);
        }

        public async Task<Update?> GetLatestUpdate(ApplicationInfo app)
        {
            string url = $"Updates/GetLatest/{app.Id}";
            var update = await _client.GetFromJsonAsync<Update>(url);
            return update;
        }

        public async Task<IEnumerable<Update>?> GetUpdates(ApplicationInfo app, DateTime releaseDate)
        {
            string time = releaseDate.ToString("yyyy-MM-ddTHH:mm:ss");
            string url = $"Updates/GetUpdates/{app.Id}?releaseDate={time}";
            var update = await _client.GetFromJsonAsync<IEnumerable<Update>>(url);
            return update;
        }
        public async Task<IEnumerable<ApplicationInfo>> Search(string name, int count, int page)
        {
            var apps = await _client.GetFromJsonAsync<IEnumerable<ApplicationInfo>>($"Applications/Search?name={name}&count={count}&page={page}");
            return apps ?? [];
        }

    }
}
