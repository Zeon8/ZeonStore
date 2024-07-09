using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Threading;
using System;
using ZeonStore.Common;
using ZeonStore.Services;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;

namespace ZeonStore.ViewModels
{
    public partial class InstallationViewModel : ViewModelBase
    {
        public ApplicationInfo Application { get; }

        [ObservableProperty] private bool _isUpdateAvailable;

        [ObservableProperty] private bool _isPaused;

        [ObservableProperty] private bool _isInstalled;

        [ObservableProperty] private bool _isInstalling;

        [ObservableProperty] private double _installProgress;

        private readonly ApplicationInstaller _installer;
        private readonly IInstalledAppsRepository _repository;
        private readonly IMessageBox _messageBox;
        private readonly ApplicationUpdater _updater;

        private CancellationTokenSource? _source;

        public InstallationViewModel(ApplicationDetailedInfo application,
            IApplicationsService applicationsService, IInstalledAppsRepository repository, IMessageBox messageBox)
        {
            Application = application;
            _repository = repository;
            _messageBox = messageBox;
            _installer = new ApplicationInstaller(application, repository, applicationsService);
            _updater = new ApplicationUpdater(_installer, application, _repository, applicationsService);
            
        }

        public async Task CheckUpdates()
        {
            if (IsInstalled)
            {
                IsUpdateAvailable = await _updater.IsUpdateAvailable();
            }
        }

        [RelayCommand]
        private void TogglePause()
        {
            IsPaused = !IsPaused;
            _installer.SetPaused(IsPaused);
        }

        private void InstallProgressChanged(double progress)
        {
            InstallProgress = progress;
        }

        [RelayCommand]
        private async Task Cancel()
        {
            if (_source is not null)
                await _source.CancelAsync();

            IsInstalling = false;
        }

        [RelayCommand]
        public async Task Install()
        {
            PrepareInstallation();
            bool succeed = false;
            bool handled = false;
            try
            {
                succeed = await _installer.Install(_source!.Token);
            }
            catch (Exception exception)
            {
                if (Debugger.IsAttached)
                    throw;
                if(exception is not TaskCanceledException)
                    await _messageBox.ShowInstallationError(exception);
                handled = true;
            }
            if (succeed)
                IsInstalled = true;
            else if(!handled && !_source.IsCancellationRequested)
                await _messageBox.ShowUnknownInstallationError();
            FinishInstallation();
        }


        [RelayCommand]
        public async Task Uninstall()
        {
            await _installer.Uninstall();
            _repository.Remove(Application);

            IsInstalled = false;
        }

        [RelayCommand]
        private void Run()
        {
            var fileName = _repository.GetExebutable(Application);
            if(fileName is not null)
                AppRunner.Run(Application.Id, fileName);
        }

        [RelayCommand]
        private async Task Update()
        {
            PrepareInstallation();
            bool succeed = false;
            bool handled = false;
            try
            {
                succeed = await _updater.Update(_source.Token);
            }
            catch (Exception exception)
            {
                if (Debugger.IsAttached)
                    throw;
                await _messageBox.ShowInstallationError(exception);
                handled = true;
            }
            if (succeed)
                IsUpdateAvailable = false;
            else if(!handled)
                await _messageBox.ShowUnknownInstallationError();
            FinishInstallation();
        }

        [MemberNotNull(nameof(_source))]
        private void PrepareInstallation()
        {
            IsInstalling = true;
            _source = new CancellationTokenSource();
            _installer.InstallProgressChanged += InstallProgressChanged;
        }

        private void FinishInstallation()
        {
            _installer.InstallProgressChanged -= InstallProgressChanged;
            IsInstalling = false;
        }
    }
}
