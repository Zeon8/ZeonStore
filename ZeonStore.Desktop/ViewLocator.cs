﻿using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ZeonStore.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZeonStore.Desktop
{
    public class ViewLocator : IDataTemplate
    {
        public bool SupportsRecycling => false;

        public bool Match(object? data) => data is ViewModelBase;

        public Control? Build(object? data)
        {
            if (data is null)
                return null;

            var name = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
            var type = Type.GetType(name);

            if (type != null)
            {
                var control = (Control)Activator.CreateInstance(type)!;
                control.DataContext = data;
                return control;
            }

            return new TextBlock { Text = "Not Found: " + name };
        }
    }
}
