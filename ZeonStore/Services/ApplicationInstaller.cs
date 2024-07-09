using Downloader;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZeonStore.Common;

namespace ZeonStore.Services
{
    public class ApplicationInstaller : IApplicationInstaller
    {
        public event InstallProgressChangedDelegate? InstallProgressChanged;
        
        private bool _paused;

        private readonly DownloadService _downloader = new DownloadService(new()
        {
            ClearPackageOnCompletionWithFailure = true
        });

        private readonly ApplicationDetailedInfo _application;
        private readonly IInstalledAppsRepository _repository;
        private readonly IApplicationsService _applicationsService;

        public ApplicationInstaller(ApplicationDetailedInfo application, 
            IInstalledAppsRepository repository, 
            IApplicationsService applicationsService)
        {
            _downloader.DownloadProgressChanged += (sender, e) => InstallProgressChanged?.Invoke(e.ProgressPercentage);
            _application = application;
            _repository = repository;
            _applicationsService = applicationsService;
        }

        public async Task<bool> Install(CancellationToken token)
        {
            var update = await _applicationsService.GetLatestUpdate(_application);
            DownloadFileInfo download = update.InstallFiles.GetForCurrentOS() 
                ?? throw new PlatformNotSupportedException();

            var succeed = await InstallFromUrl(download.DownloadUrl, token);
            if(succeed)
                _repository.Add(_application, update);
            return succeed;
        }

        public async Task<bool> InstallFromUrl(string downloadUrl, CancellationToken token)
        {
            _paused = false;
            using var stream = await _downloader.DownloadFileTaskAsync(downloadUrl, token);
            if (_downloader.Status != DownloadStatus.Completed)
            {
                await _downloader.Clear();
                _downloader.Package.Clear();
                return false;
            }
            await ExtractArchive(stream, token);
            return true;
        }

        public Task Uninstall()
        {
            var path = Path.Combine(Constants.InstalledAppsDirectory, _application.Id.ToString());
            
            return Task.Run(() =>
            {
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
            });
        }

        private async Task ExtractArchive(Stream stream, CancellationToken token)
        {
            using var source = new ZipArchive(stream);
            for (int i = 0; i < source.Entries.Count; i++)
            {
                while(_paused)
                    await Task.Delay(20, token);
                    
                ZipArchiveEntry entry = source.Entries[i];
                var path = Path.Combine(Directory.GetCurrentDirectory(), Constants.InstalledAppsDirectory, _application.Id.ToString(), entry.FullName);
                if (Path.GetFileName(path).Length == 0)
                    Directory.CreateDirectory(path);
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                    await Task.Run(() => entry.ExtractToFile(path, true), token);
                }

                double entryNumber = i + 1;
                double progress = entryNumber / source.Entries.Count * 100;
                InstallProgressChanged?.Invoke(progress);
            }
        }

        public void SetPaused(bool paused)
        {
            if(_downloader.Status != DownloadStatus.Completed)
            {
                if (paused)
                    _downloader.Pause();
                else
                    _downloader.Resume();
            }
            else
            {
                _paused = paused;
            }
        }
    }
}
