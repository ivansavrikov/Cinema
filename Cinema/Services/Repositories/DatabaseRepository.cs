using Cinema.Models.Database;
using Cinema.Models.Database.Entities;
using Cinema.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Cinema.Services.Repositories
{
    public class DatabaseRepository
    {
        private DatabaseContext _dbContext;
        public DatabaseRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddFilmAsync(FilmEntity film)
        {
            await _dbContext.Films.AddAsync(film);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<FilmEntity> GetFilmByIdAsync(int id)
        {
            var film = await _dbContext.Films
                .Where(f => f.Id == id)
                .FirstOrDefaultAsync();
            return film;
        }

        public async Task<List<FilmEntity>> GetAllFilms()
        {
            return await _dbContext.Films.ToListAsync();
        }

        public async Task AddUserAsync(UserEntity user)
        {
           await _dbContext.Users.AddAsync(user);
           await _dbContext.SaveChangesAsync();
        }

        public async Task AddGenreAsync(GenreEntity genre)
        {
            await _dbContext.Genres.AddAsync(genre);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<GenreEntity> GetGenreByNameAsync(string name)
        {
            return await _dbContext.Genres
                .Where(g => g.Title == name)
                .FirstOrDefaultAsync();
        }

        public async Task<List<FilmEntity>> GetUserFilms(int userId = 1)
        {
            var user = await _dbContext.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            var films = new List<FilmEntity>();
            foreach (var uf in user.UserFilms)
                films.Add(uf.Film);

            return films;
        }

        public async Task AddFilmToUser(FilmEntity film, int userId = 1)
        {
            var user = await _dbContext.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            user.UserFilms.Add(new UserFilm { User = user, Film = film });
            await _dbContext.SaveChangesAsync(); 
        }
    }
}
