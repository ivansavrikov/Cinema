using Cinema.Helpers;
using Cinema.Models.Entities;
using Cinema.Services;
using Cinema.Services.Repositories;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cinema.ViewModels
{
    public class FilmsViewModel : BaseViewModel
    {
        private readonly CommandAggregator _commandAggregator;
        private readonly DatabaseRepository _repository;
        private readonly KinopoiskApiService _api;
        public ObservableCollection<FilmEntity> Films { get; set; } = new ObservableCollection<FilmEntity>();
        public ICommand AddFilmToFavoritesCommand => _commandAggregator.GetCommand("AddFilmToFavoritesCommand");
        public ICommand GetFilmInfoCommand => _commandAggregator.GetCommand(nameof(GetFilmInfoCommand));

        public FilmsViewModel(KinopoiskApiService api, CommandAggregator commandAggregator, DatabaseRepository repository)
        {
            _repository = repository;
            _api = api;
            _commandAggregator = commandAggregator;
            _commandAggregator.RegisterCommand(nameof(GetFilmInfoCommand), new RelayCommand(GetFilmInfo));

            Task.Run(async () => await LoadFilmsAsync()).GetAwaiter().GetResult();
        }

        public async Task LoadFilmsAsync()
        {
            var films = await _repository.GetAllFilmsAsync();
            foreach (var f in films)
                Films.Add(f);
        }

        public void GetFilmInfo(object film)
        {
            _commandAggregator.GetCommand("NavigateCommand").Execute("filmInfoPage");
            _commandAggregator.GetCommand("SetCurrentFilmCommand").Execute(film);
        }
    }
}
