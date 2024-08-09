using Cinema.Helpers;
using Cinema.Models.Entities;
using Cinema.Services;
using Cinema.Services.Repositories;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cinema.ViewModels
{
    public class FavoritesViewModel : BaseViewModel
    {
        private readonly DatabaseRepository _repository;
        private readonly CommandAggregator _commandAggregator;
        public ObservableCollection<FilmEntity> FavoritesFilms { get; set; } = [];
        public ICommand AddFilmToFavoritesCommand => _commandAggregator.GetCommand(nameof(AddFilmToFavoritesCommand));
        public ICommand GetFilmInfoCommand => _commandAggregator.GetCommand(nameof(GetFilmInfoCommand));
        public FavoritesViewModel(CommandAggregator commandAggregator, DatabaseRepository repository)
        {
            _repository = repository;
            _commandAggregator = commandAggregator;
            _commandAggregator.RegisterCommand(nameof(AddFilmToFavoritesCommand), new RelayCommand(AddFilmToFavorites));
            _ = LoadUserFilms();
        }

        public async void AddFilmToFavorites(object film)
        {
            if (film is FilmEntity filmEntity)
            {
                if (await _repository.IsFilmAddedByUser(filmEntity))
                    await _repository.DeleteFilmFromUserAsync(filmEntity);
                else if (await _repository.IsFilmAddedAsync(filmEntity))
                    await _repository.AddFilmToUserAsync(filmEntity);

                await LoadUserFilms();
            }
        }

        public async Task LoadUserFilms()
        {
            FavoritesFilms = [];
            var userFilms = await _repository.GetUserFilmsAsync();
            foreach (var film in userFilms)
                FavoritesFilms.Add(film);
        }
    }
}