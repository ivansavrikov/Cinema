using Cinema.Models.Database.Entities;
using Cinema.Services.Repositories;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Cinema.Services
{
    public class DatabaseFillService(KinopoiskRepository kinopoiskRepository, DatabaseRepository databaseRepository)
    {
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

            await databaseRepository.AddUserAsync(user);
        }

        public async Task FillGenresAsync()
        {
            var genres = await kinopoiskRepository.GetAllGenresAsync();
            foreach (var genre in genres)
                await databaseRepository.AddGenreAsync(genre);
        }

        public async Task FillFilmsAsync()
        {
            var films = await kinopoiskRepository.GetAllFilmsAsync();
            foreach (var film in films)
            {
                await databaseRepository.AddFilmAsync(film);
            }
            //var film = await kinopoiskRepository.GetFilmByIdAsync(301);
            //await databaseRepository.AddFilmAsync(film);
        }
    }
}