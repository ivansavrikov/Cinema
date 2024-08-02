using Cinema.Abstractions;
using Cinema.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Repositories
{
    public class KinopoiskRepository : ICinemaRepository
    {
        public async Task<CinematicContent> GetFilmInfoByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CinematicContent>> GetFilmsOnPageAsync(int page)
        {
            throw new NotImplementedException();
        }
    }
}
