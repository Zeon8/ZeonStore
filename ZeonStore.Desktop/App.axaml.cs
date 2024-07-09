using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using ZeonStore.Views;
using ZeonStore.ViewModels;
using System.Globalization;
using System;
using ZeonStore.Desktop;

namespace ZeonStore;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);
        Desktop.Locales.Resources.Culture = CultureInfo.CurrentCulture;
        var messageBox = new MessageBox();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(messageBox)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
