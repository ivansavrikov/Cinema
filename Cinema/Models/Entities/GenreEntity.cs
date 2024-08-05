using System.Collections.Generic;

namespace Cinema.Models.Entities
{
    public class GenreEntity
    {
        public int Id { get; set; }
        public string KinoposikId { get; set; }
        public string Title { get; set; }
        public List<FilmEntity> Films { get; set; } = new List<FilmEntity>();
    }
}
