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
        public List<GenreEntity> Genres { get; set; } = new List<GenreEntity>();
    }
}
