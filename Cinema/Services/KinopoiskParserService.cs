using Cinema.Models.Entities;
using System.Collections.Generic;

namespace Cinema.Services
{
    public class KinopoiskParserService
    {
        public FilmEntity ParseSingleFilm(string jsonString)
        {
            return new FilmEntity();
        }

        public List<FilmEntity> ParseFilmCollection(string jsonString)
        {
            return new List<FilmEntity>();
        }

        public List<GenreEntity> ParseGenres(string jsonString)
        {
            return new List<GenreEntity>();
        }
    }
}
