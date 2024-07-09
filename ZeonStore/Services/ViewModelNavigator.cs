using System;
using ZeonStore.ViewModels;

namespace ZeonStore.Services
{
    public class ViewModelNavigator : IViewModelNavigator
    {
        private readonly Action<ViewModelBase> _setView;

        public ViewModelNavigator(Action<ViewModelBase> setView)
        {
            _setView = setView;
        }

        public void SetView(ViewModelBase viewModel) => _setView(viewModel);
    }
}
