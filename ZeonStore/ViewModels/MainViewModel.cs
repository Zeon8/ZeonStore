using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ZeonStore.Common;
using ZeonStore.Services;
using ZeonStore.Services.Loaders;

namespace ZeonStore.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        public IEnumerable<InstallationViewModel> Installations { get; }

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty] 
        private ObservableCollection<ApplicationInfo> _applications = new();

        [ObservableProperty] 
        private bool _isGridView;

        private const int LoadAppsCount = 5;

        private IApplicationsLoader _loader;
        private readonly SearchApplicationsLoader _searchLoader;
        private readonly DefaultLoader _defaultLoader;

        private readonly List<InstallationViewModel> _installations = new();

        private readonly ApplicationsService _appsService = new();

        private readonly InstalledAppsRepository _repository = new(); 

        private readonly IViewModelNavigator _navigator;
        private readonly IMessageBox _messageBox;

        private int _page;

        public MainViewModel(IViewModelNavigator navigator, IMessageBox messageBox)
        {
            Installations = _installations.Where(i => i.IsInstalled || i.IsInstalling);
            _navigator = navigator;
            _messageBox = messageBox;
            _searchLoader = new SearchApplicationsLoader(_appsService);
            _defaultLoader = new DefaultLoader(_appsService);
            _loader = _defaultLoader;
            
            Task.Run(Load).ContinueWith(_ => LoadInstalledApps());
        } 

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public MainViewModel() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


        public async Task Load()
        {
            IEnumerable<ApplicationInfo> apps = await _loader.GetApplications(LoadAppsCount, 0);
            AddApplications(apps);
        }

        public void AddApplications(IEnumerable<ApplicationInfo> apps)
        {
            foreach (var app in apps)
            {
                Applications.Add(app);
            }
        }

        public async Task LoadMore()
        {
            _page++;
            var apps = await _loader.GetApplications(LoadAppsCount, _page);
            AddApplications(apps);
        }

        private void ResetPage() => _page = 0;

        partial void OnSearchTextChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
                _loader = _defaultLoader;
            else
            {
                if (_loader is not SearchApplicationsLoader)
                    _loader = _searchLoader;
                _searchLoader.ApplicationName = value;
            }
            ResetPage();
            Applications.Clear();
            Task.Run(Load);
        }

        private async Task LoadInstalledApps()
        {
            _repository.Load();
            foreach (var app in _repository.Apps)
            {
                var detailed = await _appsService.GetDetailedInfo(app.Id);
                if (detailed is not null)
                {
                    var installation = new InstallationViewModel(detailed, _appsService, _repository, _messageBox)
                    {
                        IsInstalled = true
                    };
                    await installation.CheckUpdates();
                    _installations.Add(installation);
                }
            };
            OnPropertyChanged(nameof(Installations));
        }

        [RelayCommand]
        private async Task Open(ApplicationInfo app)
        {
            var detailed = await _appsService.GetDetailedInfo(app);
            if (detailed is not null)
            {
                var installation = _installations.FirstOrDefault(i => i.Application.Id == detailed.Id);
                if (installation is null)
                {
                    installation = new InstallationViewModel(detailed, _appsService, _repository, _messageBox);
                    _installations.Add(installation);
                }
                else
                {
                    if (installation.IsInstalled)
                        await installation.CheckUpdates();
                }
                _navigator.SetView(new AppViewModel(detailed, installation, GoBack));
            }
        }

        private void GoBack()
        {
            _navigator.SetView(this);
        }
    }
}