using Cinema.Services;
using Cinema.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Cinema.Views
{
    public sealed partial class FilmsPage : Page
    {
        public FilmsPage()
        {
            this.InitializeComponent();
            this.DataContext = App.ServiceProvider.GetService(typeof(FilmsViewModel));

            var api = App.ServiceProvider.GetRequiredService<KinopoiskApiService>();
            var film = Task.Run(async () => await api.GetFilmInfoByIdAsync(301)).GetAwaiter().GetResult();
            var films = Task.Run(async () => await api.GetFilmsOnPageAsync(1)).GetAwaiter().GetResult();
        }
    }
}