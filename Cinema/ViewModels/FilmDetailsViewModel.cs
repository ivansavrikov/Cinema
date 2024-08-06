using Cinema.Helpers;
using Cinema.Models.Entities;
using Cinema.Services;
using Cinema.Services.Repositories;
using System.Windows.Input;

namespace Cinema.ViewModels
{
    public class FilmDetailsViewModel : BaseViewModel
    {
        private readonly KinopoiskApiService _api;
        private readonly CommandAggregator _commandAggregator;
        private readonly DatabaseRepository _repository;

        private FilmEntity _currentFilm;
        public FilmEntity CurrentFilm
        {
            get => _currentFilm;

            set
            {
                if (value == _currentFilm)
                    return;
                _currentFilm = value;
                OnPropertyChanged();
            }
        }

        public ICommand SetCurrentFilmCommand => _commandAggregator.GetCommand(nameof(SetCurrentFilmCommand));
        public ICommand AddFilmToFavoritesCommand => _commandAggregator.GetCommand("AddFilmToFavoritesCommand");
        public FilmDetailsViewModel(CommandAggregator commandAggregator, KinopoiskApiService api, DatabaseRepository repository)
        {
            _repository = repository;
            _api = api;
            _commandAggregator = commandAggregator;
            _commandAggregator.RegisterCommand(nameof(SetCurrentFilmCommand), new RelayCommand(SetCurrentFilm));
        }

        public async void SetCurrentFilm(object film)
        {
            var detailedFilm = await _api.GetFilmInfoByIdAsync((film as FilmEntity).KinopoiskId);
            CurrentFilm = await _repository.UpdateFilmAsync(detailedFilm);
        }
    }
}
