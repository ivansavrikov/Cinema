using System.Collections.Generic;

namespace Cinema.Models.Entities
{
    public class FilmEntity
    {
        public int Id { get; set; }
        public int KinoposikId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Year { get; set; }
        public string PosterUrl { get; set; }
        public List<GenreEntity> Genres { get; set; } = new List<GenreEntity>();
    }
}
