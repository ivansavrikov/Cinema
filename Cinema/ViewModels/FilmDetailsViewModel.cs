using Cinema.Helpers;
using Cinema.Models.Entities;
using Cinema.Services;
using Cinema.Services.Repositories;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace Cinema.ViewModels
{
    public class FilmDetailsViewModel : BaseViewModel
    {
        private readonly KinopoiskRepository _kinopoiskRepository;
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

        private BitmapImage _poster;

        public BitmapImage Poster
        {
            get => _poster;
            set
            {
                if (value == _poster)
                    return;
                _poster = value;
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

        public FilmDetailsViewModel(CommandAggregator commandAggregator, KinopoiskRepository kinopoiskRepository, DatabaseRepository repository)
        {
            _repository = repository;
            _kinopoiskRepository = kinopoiskRepository;
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
                if(film is FilmEntity filmEntity)
                {
                    //if(filmEntity.Description == null)
                    //{
                    //    var detailedFilm = await _kinopoiskRepository.GetFilmByIdAsync(filmEntity.KinopoiskId);
                    //    CurrentFilm = await _repository.UpdateFilmAsync(detailedFilm);
                    //}
                    //else
                    //{
                    //    CurrentFilm = filmEntity;
                    //}
                    CurrentFilm = filmEntity;
                    ToggleFavoriteButton(filmEntity);
                    Poster = await BytesConverter.ToBitmapImage(CurrentFilm.PosterImage);
                }
            }
            catch (System.Exception e)
            {
                Debug.WriteLine("Проблемы с подключением к интернету, или с API");
                throw e;
            }
        }
    }
}
