using Cinema.Helpers;
using Cinema.Models.Entities;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Cinema.ViewModels
{
    public class FilmViewModel : BaseViewModel
    {
        public FilmEntity Film { get; set; }
        public string Title { get; set; }

        private string _year;
        public string Year
        {
            get => _year;
            set
            {
                _year = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _poster;
        public BitmapImage Poster
        {
            get => _poster;
            set
            {
                _poster = value;
                OnPropertyChanged();
            }
        }

        public FilmViewModel(FilmEntity film)
        {
            Film = film;
            Title = Film.Title;
            Year = Film.Year.ToString();
            _ = InitializeAsync();
        }
        public async Task InitializeAsync()
        {
            Poster = await BytesConverter.ToBitmapImage(Film.PosterImage);
        }
    }
}
