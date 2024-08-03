using Cinema.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Cinema.Views
{
    public sealed partial class FilmsPage : Page
    {
        public FilmsPage()
        {
            this.InitializeComponent();
            this.DataContext = App.ServiceProvider.GetService(typeof(FilmsViewModel));
        }
    }
}
