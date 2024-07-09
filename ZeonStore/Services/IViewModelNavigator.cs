using ZeonStore.ViewModels;

namespace ZeonStore.Services
{
    public interface IViewModelNavigator
    {
        void SetView(ViewModelBase viewModelBase);
    }
}
