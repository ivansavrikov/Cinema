using Cinema.Models.Database.Entities;
using System.Collections.Generic;

namespace Cinema.Models.Entities
{
    public class FilmEntity
    {
        public int Id { get; set; }
        public int KinopoiskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public short Year { get; set; }
        public string PosterUrl { get; set; }
        public List<FilmGenre> FilmGenres { get; set; } = new List<FilmGenre>();
        public List<UserFilm> UserFilms { get; set; } = new List<UserFilm>();
    }
}