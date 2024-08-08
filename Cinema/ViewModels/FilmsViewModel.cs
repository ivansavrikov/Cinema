using Cinema.Helpers;
using Cinema.Models.Entities;
using Cinema.Services;
using Cinema.Services.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cinema.ViewModels
{
    public class FilmsViewModel : BaseViewModel
    {
        private readonly CommandAggregator _commandAggregator;
        private readonly DatabaseRepository _repository;
        public ObservableCollection<FilmEntity> Films { get; set; } = [];
        public ObservableCollection<FilmViewModel> FilmsViewModels { get; set; } = [];

        public ICommand AddFilmToFavoritesCommand => _commandAggregator.GetCommand("AddFilmToFavoritesCommand");
        public ICommand GetFilmInfoCommand => _commandAggregator.GetCommand(nameof(GetFilmInfoCommand));

        public FilmsViewModel(CommandAggregator commandAggregator, DatabaseRepository repository)
        {
            _repository = repository;
            _commandAggregator = commandAggregator;
            _commandAggregator.RegisterCommand(nameof(GetFilmInfoCommand), new RelayCommand(GetFilmInfo));

            _ = LoadFilmsAsync();
        }

        public async Task LoadFilmsAsync()
        {
            var films = await _repository.GetAllFilmsAsync();
            foreach (var f in films)
                FilmsViewModels.Add(new FilmViewModel(f));
        }

        public void GetFilmInfo(object film)
        {
            if (film == null)
            {
                throw new Exception("film is null");
            }

            _commandAggregator.GetCommand("NavigateCommand").Execute("filmInfoPage");
            _commandAggregator.GetCommand("SetCurrentFilmCommand").Execute(film);
        }
    }
}