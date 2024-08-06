using Cinema.Models.Database;
using Cinema.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
    }
}
