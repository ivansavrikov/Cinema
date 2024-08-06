using System.Collections.Generic;

namespace Cinema.Models.Entities
{
    public class GenreEntity
    {
        public int Id { get; set; }
        public string KinopoiskId { get; set; }
        public string Title { get; set; }

        public List<FilmGenreEntity> FilmGenres { get; set; } = new List<FilmGenreEntity>();
    }
}