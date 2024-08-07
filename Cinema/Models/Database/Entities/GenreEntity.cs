using System.Collections.Generic;

namespace Cinema.Models.Entities
{
    public class GenreEntity
    {
        public int Id { get; set; }
        public int KinopoiskId { get; set; }
        public string Title { get; set; }

        public List<FilmGenre> FilmGenres { get; set; } = new List<FilmGenre>();
    }
}