using Cinema.Helpers;
using Cinema.Models.Entities;
using Cinema.Services;
using Cinema.Services.Repositories;
using System.Diagnostics;
using System.Threading.Tasks;
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

        private string _favouriteButtonText = "В Избранное";
        public string FavouriteButtonText
        {
            get => _favouriteButtonText;
            set
            {
                _favouriteButtonText = value;
                OnPropertyChanged();
            }

        }

        public ICommand SetCurrentFilmCommand => _commandAggregator.GetCommand(nameof(SetCurrentFilmCommand));
        public ICommand AddFilmToFavoritesCommand { get; set; }
        public ICommand NavigateCommand => _commandAggregator.GetCommand("NavigateCommand");

        public void ToggleFavoriteButton(FilmEntity film)
        {
            var isAdded = Task.Run(async () => 
                await _repository.IsFilmAddedByUser(film))
                .GetAwaiter()
                .GetResult();
            if (isAdded)
                FavouriteButtonText = "Убрать из Избранного";
            else
                FavouriteButtonText = "В Избранное";
        }

        public FilmDetailsViewModel(CommandAggregator commandAggregator, KinopoiskApiService api, DatabaseRepository repository)
        {
            _repository = repository;
            _api = api;
            _commandAggregator = commandAggregator;
            _commandAggregator.RegisterCommand(nameof(SetCurrentFilmCommand), new RelayCommand(SetCurrentFilm));
            AddFilmToFavoritesCommand = new RelayCommand(AddFilmToFavourite);
        }

        public void AddFilmToFavourite(object film)
        {
            _commandAggregator.GetCommand("AddFilmToFavoritesCommand").Execute(film);
            ToggleFavoriteButton(film as FilmEntity);
        }

        public async void SetCurrentFilm(object film)
        {
            try
            {
                var detailedFilm = await _api.GetFilmInfoByIdAsync((film as FilmEntity).KinopoiskId);
                CurrentFilm = await _repository.UpdateFilmAsync(detailedFilm);
                ToggleFavoriteButton(film as FilmEntity);
            }
            catch (System.Exception)
            {
                Debug.WriteLine("Проблемы с подключением к интернету, или с API");
            }
        }
    }
}
