using Cinema.Helpers;
using Cinema.Models.Entities;
using Cinema.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Cinema.ViewModels
{
    public class FilmsViewModel : BaseViewModel
    {
        public event Action<FilmEntity> AddedToFavoritesEvent;
        public ICommand AddToFavoritesCommand { get; }
        public ObservableCollection<FilmEntity> Films { get; set; } = new ObservableCollection<FilmEntity>();

        private KinopoiskApiService _api;
        public FilmsViewModel(KinopoiskApiService api)
        {
            _api = api;
            Task.Run(async () => await LoadFilmsAsync()).GetAwaiter().GetResult();
            AddToFavoritesCommand = new RelayCommand(AddToFavorites);
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
    }
}
