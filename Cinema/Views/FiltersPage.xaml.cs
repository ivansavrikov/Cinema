using Cinema.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;

namespace Cinema.Views
{
    public sealed partial class FiltersPage : Page
    {
        public FiltersPage()
        {
            this.InitializeComponent();
            this.DataContext = App.ServiceProvider.GetRequiredService<FiltersViewModel>();
        }
    }
}
