﻿using Cinema.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;

namespace Cinema.Views
{
    public sealed partial class FilmsPage : Page
    {
        public FilmsPage()
        {
            this.InitializeComponent();
            this.DataContext = App.ServiceProvider.GetRequiredService<FilmsViewModel>();
        }
    }
}