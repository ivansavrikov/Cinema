using Cinema.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;

namespace Cinema.Views
{
    public sealed partial class FavoritesPage : Page
    {
        public FavoritesPage()
        {
            this.InitializeComponent();
            this.DataContext = App.ServiceProvider.GetRequiredService<FavoritesFilmsViewModel>();
        }
    }
}
