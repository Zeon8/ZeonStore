using CommunityToolkit.Mvvm.Input;
using System;
using ZeonStore.Common;

namespace ZeonStore.ViewModels
{
    public partial class AppViewModel : ViewModelBase
    {
        public ApplicationDetailedInfo Application { get; }

        public InstallationViewModel Installation { get; }

        private readonly Action _backToMenu;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public AppViewModel() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public AppViewModel(ApplicationDetailedInfo application, 
            InstallationViewModel installation, Action backToMenu)
        {
            Application = application;
            Installation = installation;
            _backToMenu = backToMenu;
        }

        [RelayCommand]
        private void ReturnBack()
        {
            _backToMenu.Invoke();
        }
    }
}
