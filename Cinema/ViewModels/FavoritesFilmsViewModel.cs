using Cinema.Helpers;
using Cinema.Models.Entities;
using Cinema.Services;
using Cinema.Services.Repositories;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;

namespace Cinema.ViewModels
{
    public class FavoritesFilmsViewModel : BaseViewModel
    {
        private DatabaseRepository _repository;
        private CommandAggregator _commandAggregator;
        public ObservableCollection<FilmEntity> FavoritesFilms { get; set; } = new ObservableCollection<FilmEntity>();
        public ICommand AddFilmToFavoritesCommand => _commandAggregator.GetCommand(nameof(AddFilmToFavoritesCommand));
        public ICommand GetFilmInfoCommand => _commandAggregator.GetCommand(nameof(GetFilmInfoCommand));
        public FavoritesFilmsViewModel(CommandAggregator commandAggregator, DatabaseRepository repository)
        {
            _repository = repository;
            _commandAggregator = commandAggregator;
            _commandAggregator.RegisterCommand(nameof(AddFilmToFavoritesCommand), new RelayCommand(AddFilmToFavorites));
            Task.Run(async () => await LoadUserFilms()).GetAwaiter().GetResult();
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
            FavoritesFilms = new ObservableCollection<FilmEntity>();
            var userFilms = await _repository.GetUserFilmsAsync();
            foreach (var film in userFilms)
                FavoritesFilms.Add(film);
        }
    }
}