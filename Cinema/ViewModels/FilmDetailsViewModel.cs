using Cinema.Helpers;
using Cinema.Models.Entities;
using Cinema.Services;
using Cinema.Services.Repositories;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cinema.ViewModels
{
    public class FilmDetailsViewModel : BaseViewModel
    {
        private FilmViewModel _filmViewModel;
        public FilmViewModel FilmViewModel
        {
            get => _filmViewModel;
            set
            {
                _filmViewModel = value;
                OnPropertyChanged();
            }
        }

        private readonly KinopoiskRepository _kinopoiskRepository;
        private readonly CommandAggregator _commandAggregator;
        private readonly NavigationManager _navigationManager;
        private readonly FilmMigratorService _migrator;
        private readonly DatabaseRepository _repository;

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

        private bool _buttonIsEnabled { get; set; } = true;
        public bool FavoriteButtonIsEnabled
        {
            get => _buttonIsEnabled;
            set
            {
                _buttonIsEnabled = value;
                OnPropertyChanged();
            }
        }

        public ICommand SetCurrentFilmCommand => _commandAggregator.GetCommand(nameof(SetCurrentFilmCommand));
        public ICommand AddFilmToFavoritesCommand { get; set; }
        public ICommand NavigateCommand => new RelayCommand((object p) =>
        {
            _commandAggregator.GetCommand("NavigateCommand").Execute(_navigationManager.PreviousPage);
        });

        public void ToggleFavoriteButton(FilmEntity film)
        {
            var isAdded = Task.Run(async () => 
                await _repository.IsFilmAddedByUser(film)).GetAwaiter().GetResult();
            if (isAdded)
                FavouriteButtonText = "Убрать из Избранного";
            else
                FavouriteButtonText = "В Избранное";
        }

        public FilmDetailsViewModel(CommandAggregator commandAggregator, KinopoiskRepository kinopoiskRepository, DatabaseRepository repository, FilmMigratorService migrator, NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            _migrator = migrator;
            _repository = repository;
            _kinopoiskRepository = kinopoiskRepository;
            _commandAggregator = commandAggregator;
            _commandAggregator.RegisterCommand(nameof(SetCurrentFilmCommand), new RelayCommand(OpenFilmDetails));
            AddFilmToFavoritesCommand = new RelayCommand(AddFilmToFavourite);
        }

        public void AddFilmToFavourite(object film)
        {
            if (film == null)
                return;

            _commandAggregator.GetCommand("AddFilmToFavoritesCommand").Execute(film);
            ToggleFavoriteButton(film as FilmEntity);
        }

        public async void OpenFilmDetails(object film)
        {
            FilmViewModel = null;
            FavoriteButtonIsEnabled = true;

            if (film is FilmEntity filmEntity)
            {
                bool filmIsAdded = await _repository.IsFilmAddedAsync(filmEntity.KinopoiskId);
                if (!filmIsAdded || !filmEntity.IsFullySynchronized)
                {
                    try
                    {
                        await _migrator.MigrateFilmAsync(filmEntity.KinopoiskId);
                        filmIsAdded = true;
                        _commandAggregator.GetCommand("LoadFilmsCommand").Execute(null); //FIXME
                    }
                    catch (Exception)
                    {
                        if (!filmIsAdded)
                        {
                            ToggleFavoriteButton(filmEntity);
                            FavoriteButtonIsEnabled = false;
                            FilmViewModel = new(filmEntity);
                            return;
                        }
                    }
                }

                filmEntity = await _repository.GetFilmByKinopoiskIdAsync(filmEntity.KinopoiskId); //FIXME
                await InitializeFilmViewModelAsync(filmEntity);
                ToggleFavoriteButton(filmEntity);
            }
        }

        public async Task InitializeFilmViewModelAsync(FilmEntity film)
        {
            FilmViewModel filmViewModel = new(film);
            var genres = await _repository.GetFilmGenresAsync(film);
            StringBuilder sb = new();
            foreach (var g in genres)
                sb.Append($"{g.Title}, ");
            string genresString = sb.ToString().TrimEnd(',', ' ');
            if(!string.IsNullOrEmpty(genresString))
                filmViewModel.Genres = genresString;
            FilmViewModel = filmViewModel;
        }
    }
}
