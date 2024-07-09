using Downloader;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZeonStore.Common;

namespace ZeonStore.Services
{
    public delegate void InstallProgressChangedDelegate(double progress);

    public interface IApplicationInstaller
    {
        event InstallProgressChangedDelegate? InstallProgressChanged;

        Task<bool> Install(CancellationToken token);

        Task<bool> InstallFromUrl(string url, CancellationToken token);

        Task Uninstall();

        void SetPaused(bool paused);

    }
}
