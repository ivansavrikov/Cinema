using Cinema.Models.Entities;
using System.Collections.ObjectModel;

namespace Cinema.ViewModels
{
    public class FavoritesFilmsViewModel : BaseViewModel
    {
        public ObservableCollection<FilmEntity> FavoritesFilms { get; set; } = new ObservableCollection<FilmEntity>();
        public FavoritesFilmsViewModel(FilmsViewModel filmsViewModell)
        {
            filmsViewModell.AddedToFavoritesEvent += AddFilmToFavorites;
        }

        public void AddFilmToFavorites(FilmEntity film) => FavoritesFilms.Add(film);
    }
}
