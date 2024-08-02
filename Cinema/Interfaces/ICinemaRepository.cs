using Cinema.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Interfaces
{
    public interface ICinemaRepository
    {
        Task<IEnumerable<CinematicContent>> GetFilmsOnPageAsync(int page);
        Task<CinematicContent> GetFilmInfoByIdAsync(int id);
    }
}
