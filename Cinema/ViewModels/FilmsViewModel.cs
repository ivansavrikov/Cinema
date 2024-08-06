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
        private NavigationViewModel _navViewModel;
        public event Action<FilmEntity> AddedToFavoritesEvent;
        public event Action<FilmEntity> GetFilmInfoEvent;
        public ICommand AddToFavoritesCommand { get; }
        public ICommand GetFilmInfoCommand { get; }

        public ObservableCollection<FilmEntity> Films { get; set; } = new ObservableCollection<FilmEntity>();

        private KinopoiskApiService _api;
        public FilmsViewModel(KinopoiskApiService api, NavigationViewModel navViewModel)
        {
            _api = api;
            _navViewModel = navViewModel;
            Task.Run(async () => await LoadFilmsAsync()).GetAwaiter().GetResult();
            AddToFavoritesCommand = new RelayCommand(AddToFavorites);
            GetFilmInfoCommand = new RelayCommand(GetFilmInfo);
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
            if (AddedToFavoritesEvent == null)
                return;
            AddedToFavoritesEvent.Invoke(film as FilmEntity);
        }

        public void GetFilmInfo(object film)
        {
            _navViewModel.NavigateCommand.Execute("filmInfoPage");
            if (GetFilmInfoEvent == null)
                return;
            GetFilmInfoEvent.Invoke(film as FilmEntity);
        }
    }
}
