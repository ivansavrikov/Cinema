using Cinema.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;

namespace Cinema.Views
{
    public sealed partial class FilmInfoPage : Page
    {
        public FilmInfoPage()
        {
            this.InitializeComponent();
            this.DataContext = App.ServiceProvider.GetRequiredService<FilmInfoViewModel>();
        }
    }
}