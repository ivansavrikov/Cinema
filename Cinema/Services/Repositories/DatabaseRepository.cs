using Cinema.Models.Database;
using Cinema.Models.Database.Entities;
using Cinema.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            var filmIsAdded = await IsFilmAddedAsync(film);

            if (filmIsAdded)
            {
                Debug.WriteLine($"Добавление фильма с KinopoiskID = {film.KinopoiskId} запрещено, так как он уже содержится в базе данных");
                return;
            }    

            await _dbContext.Films.AddAsync(film);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsFilmAddedAsync(FilmEntity film)
        {
            return await _dbContext.Films
                .Where(f => f.KinopoiskId == film.KinopoiskId)
                .AnyAsync();
        }

        public async Task<FilmEntity> GetFilmByIdAsync(int id)
        {
            var film = await _dbContext.Films
                .Where(f => f.Id == id)
                .FirstOrDefaultAsync();
            return film;
        }

        public async Task<List<FilmEntity>> GetAllFilmsAsync()
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

        public async Task<List<FilmEntity>> GetUserFilmsAsync(int userId = 1)
        {
            var films = new List<FilmEntity>();

            var userFilms = await _dbContext.UserFilms.Where(uf => uf.UserId == userId).ToListAsync();
            foreach (var uf in userFilms)
                films.Add(uf.Film);

            return films;
        }

        public async Task AddFilmToUserAsync(FilmEntity film, int userId = 1)
        {
            var user = await _dbContext.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            user.UserFilms.Add(new UserFilm { User = user, Film = film });
            await _dbContext.SaveChangesAsync(); 
        }

        public async Task DeleteFilmFromUserAsync(FilmEntity film, int userId = 1)
        {
            var userFilmToDelete = await _dbContext.UserFilms
                .Where(uf => uf.UserId == userId && uf.FilmId == film.Id)
                .FirstOrDefaultAsync();

            if (userFilmToDelete == null)
                throw new Exception("Данный фильм не хранится в избранном у пользователя");

             _dbContext.UserFilms.Remove(userFilmToDelete);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsFilmAddedByUser(FilmEntity film, int userId = 1)
        {
            return await _dbContext.UserFilms
                .Where(uf => uf.UserId == userId && uf.FilmId == film.Id)
                .AnyAsync();
        }

        public async Task<FilmEntity> UpdateFilmAsync(FilmEntity updatedFilm)
        {
            var film = await _dbContext.Films
                .Where(f => f.KinopoiskId == updatedFilm.KinopoiskId)
                .FirstOrDefaultAsync();

            if (film == null)
                await AddFilmAsync(updatedFilm);

            film.Title = updatedFilm.Title;
            film.Description = updatedFilm.Description;
            film.PosterUrl = updatedFilm.PosterUrl;
            film.Year = updatedFilm.Year;

            await _dbContext.SaveChangesAsync();
            return film;
        }
    }
}
