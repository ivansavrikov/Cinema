using Cinema.Helpers;
using Cinema.Models.Entities;
using Cinema.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Cinema.ViewModels
{
    public class FavoritesFilmsViewModel : BaseViewModel
    {
        private CommandAggregator _commandAggregator;

        public ObservableCollection<FilmEntity> FavoritesFilms { get; set; } = new ObservableCollection<FilmEntity>();

        public ICommand AddFilmToFavoritesCommand => _commandAggregator.GetCommand(nameof(AddFilmToFavoritesCommand));
        public ICommand GetFilmInfoCommand => _commandAggregator.GetCommand(nameof(GetFilmInfoCommand));
        public FavoritesFilmsViewModel(CommandAggregator commandAggregator)
        {
            _commandAggregator = commandAggregator;
            _commandAggregator.RegisterCommand(nameof(AddFilmToFavoritesCommand), new RelayCommand(AddFilmToFavorites));
        }

        public void AddFilmToFavorites(object film) => FavoritesFilms.Add(film as FilmEntity);
    }
}
