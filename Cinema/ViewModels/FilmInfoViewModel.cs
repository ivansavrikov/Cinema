using Cinema.Helpers;
using Cinema.Models.Entities;
using Cinema.Services;
using System.Windows.Input;

namespace Cinema.ViewModels
{
    public class FilmInfoViewModel : BaseViewModel
    {
        private readonly KinopoiskApiService _api;
        private readonly CommandAggregator _commandAggregator;
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
        public FilmInfoViewModel(CommandAggregator commandAggregator, KinopoiskApiService api)
        {
            _api = api;
            _commandAggregator = commandAggregator;
            _commandAggregator.RegisterCommand(nameof(SetCurrentFilmCommand), new RelayCommand(SetCurrentFilm));
        }

        public async void SetCurrentFilm(object film)
        {
            var detailedFilm = await _api.GetFilmInfoByIdAsync((film as FilmEntity).KinopoiskId);
            CurrentFilm = detailedFilm;
        }
    }
}
