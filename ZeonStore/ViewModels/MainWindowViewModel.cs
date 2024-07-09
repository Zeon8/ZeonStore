using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeonStore.Services;

namespace ZeonStore.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly IViewModelNavigator _navigator;

        [ObservableProperty]
        private ViewModelBase _currentView;

        public MainWindowViewModel(IMessageBox messageBox)
        {
            _navigator = new ViewModelNavigator(vm => CurrentView = vm);
            _currentView = new MainViewModel(_navigator, messageBox);
        }
    }
}
