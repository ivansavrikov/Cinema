using Cinema.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;

namespace Cinema.Views
{
    public sealed partial class FilmDetailsPage : Page
    {
        public FilmDetailsPage()
        {
            this.InitializeComponent();
            this.DataContext = App.ServiceProvider.GetRequiredService<FilmDetailsViewModel>();
        }
    }
}