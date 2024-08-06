using System.Collections.Generic;

namespace Cinema.Models.Entities
{
    public class FilmGenreEntity
    {
        public int FilmId { get; set; }
        public int GenreId { get; set; }
        public FilmEntity Film { get; set; }
        public GenreEntity Genre { get; set; }
    }
}
