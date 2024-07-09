using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ZeonStore.Common;
using ZeonStore.ViewModels;

namespace ZeonStore.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        ListBox1.TemplateApplied += ListBox1_TemplateApplied;
    }

    private void ListBox1_TemplateApplied(object? sender, Avalonia.Controls.Primitives.TemplateAppliedEventArgs e)
    {
        var scrollViewer = ListBox1.GetTemplateChildren()
            .OfType<ScrollViewer>().Single();

        scrollViewer.ScrollChanged += ScrollView_ScrollChanged;
    }

    private static T? GetDataContext<T>(TappedEventArgs e)
    {
        StyledElement? source = e.Source as StyledElement;
        while (source is not null && source.DataContext is not T)
        {
            source = source.Parent;
        }

        return (T?)source?.DataContext;
    }

    private void ListBox_DoubleTapped(object? sender, TappedEventArgs e)
    {
        var application = GetDataContext<ApplicationInfo>(e);
        if (DataContext is MainViewModel viewModel && application is not null)
            viewModel.OpenCommand.Execute(application);
    }

    private void ListBox_DoubleTapped_1(object? sender, TappedEventArgs e)
    {
        var installation = GetDataContext<InstallationViewModel>(e);
        if(DataContext is MainViewModel viewModel && installation is not null)
            viewModel.OpenCommand.Execute(installation.Application);
    }

    private void ScrollView_ScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        var scroller = (ScrollViewer)sender;
        if(scroller.Offset.Y == scroller.ScrollBarMaximum.Y
            && DataContext is MainViewModel viewModel)
        {
            Task.Run(viewModel.LoadMore);
        }
    }
}
