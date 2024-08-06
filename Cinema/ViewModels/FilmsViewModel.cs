using Cinema.Helpers;
using Cinema.Models.Entities;
using Cinema.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cinema.ViewModels
{
    public class FilmsViewModel : BaseViewModel
    {
        private readonly CommandAggregator _commandAggregator;
        private readonly KinopoiskApiService _api;
        public ObservableCollection<FilmEntity> Films { get; set; } = new ObservableCollection<FilmEntity>();
        public ICommand AddToFavoritesCommand => _commandAggregator.GetCommand(nameof(AddToFavoritesCommand));
        public ICommand GetFilmInfoCommand => _commandAggregator.GetCommand(nameof(GetFilmInfoCommand));

        public FilmsViewModel(KinopoiskApiService api, CommandAggregator commandAggregator)
        {
            _api = api;
            _commandAggregator = commandAggregator;
            _commandAggregator.RegisterCommand(nameof(AddToFavoritesCommand), new RelayCommand(AddToFavorites));
            _commandAggregator.RegisterCommand(nameof(GetFilmInfoCommand), new RelayCommand(GetFilmInfo));

            Task.Run(async () => await LoadFilmsAsync()).GetAwaiter().GetResult();
        }

        public async Task LoadFilmsAsync()
        {
            for (int i = 1; i <= 5; i++)
            {
                var filmOnPage = await _api.GetFilmsOnPageAsync(i);
                foreach (var f in filmOnPage)
                    Films.Add(f);
            }
        }

        public void AddToFavorites(object film)
        {
            _commandAggregator.GetCommand("AddFilmToFavoritesCommand").Execute(film);
        }

        public void GetFilmInfo(object film)
        {
            _commandAggregator.GetCommand("NavigateCommand").Execute("filmInfoPage");
            _commandAggregator.GetCommand("SetCurrentFilmCommand").Execute(film);
        }
    }
}
