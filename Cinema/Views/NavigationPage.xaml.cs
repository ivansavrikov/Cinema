using Cinema.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Cinema
{
    public sealed partial class NavigationPage : Page
    {
        public NavigationPage()
        {
            this.InitializeComponent();
            this.DataContext = App.ServiceProvider.GetService(typeof(NavigationViewModel));
        }
    }
}