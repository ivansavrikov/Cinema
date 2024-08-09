using Cinema.Helpers;
using Cinema.Models.Entities;
using Cinema.Services;
using Cinema.Services.Repositories;
using System.Data.SqlTypes;
using System.Diagnostics;
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

        public ICommand SetCurrentFilmCommand => _commandAggregator.GetCommand(nameof(SetCurrentFilmCommand));
        public ICommand AddFilmToFavoritesCommand { get; set; }
        public ICommand NavigateCommand => _commandAggregator.GetCommand("NavigateCommand");

        public void ToggleFavoriteButton(FilmEntity film)
        {
            var isAdded = Task.Run(async () => 
                await _repository.IsFilmAddedByUser(film)).GetAwaiter().GetResult();
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

                    ToggleFavoriteButton(filmEntity);
                    FilmViewModel = new(filmEntity);
                    var genres = await _repository.GetFilmGenresAsync(filmEntity);
                    
                    StringBuilder sb = new();
                    foreach (var g in genres)
                        sb.Append($"{g.Title}, ");
                    FilmViewModel.Genres = sb.ToString().TrimEnd(',', ' ');
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
