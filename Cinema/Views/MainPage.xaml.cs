using Cinema.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Cinema
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = App.ServiceProvider.GetService(typeof(NavigationViewModel));
        }
    }
}
