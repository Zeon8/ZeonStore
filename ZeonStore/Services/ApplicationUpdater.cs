using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using ZeonStore.Common;

namespace ZeonStore.Services
{
    public class ApplicationUpdater
    {
        private readonly IApplicationInstaller _installer;
        private readonly ApplicationDetailedInfo _application;
        private readonly IInstalledAppsRepository _repository;
        private readonly IApplicationsService _applicationsService;
        private Update? _latest;

        public ApplicationUpdater(IApplicationInstaller installer,
            ApplicationDetailedInfo application,
            IInstalledAppsRepository installedAppsRepository,
            IApplicationsService applicationsService)
        {
            _installer = installer;
            _application = application;
            _repository = installedAppsRepository;
            _applicationsService = applicationsService;
        }

        
        public async Task<bool> IsUpdateAvailable()
        {
            if (!_repository.Contains(_application))
                return false;

            var update = await _applicationsService.GetLatestUpdate(_application);
            if (update is null)
                return false;
            
            _latest = update;
            var installed = _repository.GetReleaseDate(_application);
            return update.ReleaseDate != installed;
        }

        public async Task<bool> Update(CancellationToken cancellationToken)
        {
            var version = _repository.GetReleaseDate(_application);
            return await Update(version, cancellationToken);
        }

        private async Task<bool> Update(DateTime installedRelease, CancellationToken token)
        {
            IEnumerable<Update>? updates = await _applicationsService.GetUpdates(_application, installedRelease);
            if (updates is null)
                return false;

            foreach (Update update in updates)
            {
                var download = update.UpdateFiles.GetForCurrentOS();
                if (download is null)
                    continue;

                bool success = await _installer.InstallFromUrl(download.DownloadUrl, token);
                if (!success)
                    return false;
            }
            _repository.SetUpdate(_application, _latest!);
            return true;
        }
    }
}
