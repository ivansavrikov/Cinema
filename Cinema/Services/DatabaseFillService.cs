using Cinema.Models.Database.Entities;
using Cinema.Services.Repositories;
using System.Threading.Tasks;

namespace Cinema.Services
{
    public class DatabaseFillService
    {
        private readonly KinopoiskApiService _api;
        private readonly DatabaseRepository _repository;

        public DatabaseFillService(KinopoiskApiService api, DatabaseRepository repository)
        {
            _api = api;
            _repository = repository;
        }

        public async Task FillStartDataToDatabaseAsync()
        {
            await AddLocalUserAsync();
            await FillGenresAsync();
            await FillFilmsAsync();
        }

        public async Task AddLocalUserAsync()
        {
            var user = new UserEntity()
            {
                Id = 1,
                UserName = "local"
            };

            await _repository.AddUserAsync(user);
        }

        public async Task FillGenresAsync()
        {
            var genres = await _api.GetAllGenresAsync();
            foreach (var genre in genres)
                await _repository.AddGenreAsync(genre);
        }

        public async Task FillFilmsAsync()
        {
            for (int i = 1; i <= 5; i++)
            {
                var films = await _api.GetFilmsOnPageAsync(i);
                foreach (var film in films)
                {
                    await _repository.AddFilmAsync(film);
                }
            }
        }
    }
}