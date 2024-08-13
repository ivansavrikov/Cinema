using Cinema.Helpers;
using Cinema.Models.Entities;
using Cinema.Services;
using Cinema.Services.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cinema.ViewModels
{
    public class FiltersViewModel : BaseViewModel
    {
        private DatabaseRepository _dbRepository;
        private KinopoiskRepository _kinopoiskRepository;
        private CommandAggregator _commandAggregator;
        public FiltersViewModel(CommandAggregator commandAggregator, DatabaseRepository dbRepository, KinopoiskRepository kinopoiskRepository)
        {
            _dbRepository = dbRepository;
            _kinopoiskRepository = kinopoiskRepository;
            _commandAggregator = commandAggregator;
            _commandAggregator.RegisterCommand(nameof(ApplyFiltersCommand), new RelayCommand(ApplyFilters));

            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            var genres = await _dbRepository.GetAllGenresAsync();
            foreach(var g in genres)
                Genres.Add(g);
        }

        public GenreEntity SelectedGenre { get; set; }
        public ObservableCollection<GenreEntity> Genres { get; set; } = [];
        public ObservableCollection<FilmViewModel> FilmsViewModels { get; set; } = [];

        public ICommand ApplyFiltersCommand => _commandAggregator.GetCommand(nameof(ApplyFiltersCommand));
        public ICommand GetFilmInfoCommand => _commandAggregator.GetCommand(nameof(GetFilmInfoCommand));

        private bool _applingIsActive = true;
        public bool ApplingIsActive
        {
            get => _applingIsActive;
            set
            {
                _applingIsActive = value;
                OnPropertyChanged();
            }
        }

        private string _statusText;
        public string StatusText
        {
            get => _statusText;
            set
            {
                _statusText = value;
                OnPropertyChanged();
            }
        }

        public async void ApplyFilters(object p)
        {
            StatusText = "Загрузка...";
            ApplingIsActive = false;
            FilmsViewModels.Clear();
            try
            {
                var films = await _kinopoiskRepository.GetFilmsByGenreAsync(SelectedGenre.KinopoiskId);
                foreach (var film in films)
                    FilmsViewModels.Add(new FilmViewModel(film));
            }
            catch (Exception)
            {
                StatusText = "Проверьте подключение к интернету";
                ApplingIsActive = true;
                return;
            }
            ApplingIsActive = true;
            StatusText = string.Empty;
            if (FilmsViewModels.Count == 0)
                StatusText = "Нет фильмов с данным жанром :(";
        }
    }
}
