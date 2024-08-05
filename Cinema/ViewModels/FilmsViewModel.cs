using Cinema.Models.Entities;
using Cinema.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Cinema.ViewModels
{
    public class FilmsViewModel : BaseViewModel
    {
        public ObservableCollection<FilmEntity> Films { get; set; } = new ObservableCollection<FilmEntity>();

        private KinopoiskApiService _api;
        public FilmsViewModel(KinopoiskApiService api)
        {
            _api = api;
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
    }
}
