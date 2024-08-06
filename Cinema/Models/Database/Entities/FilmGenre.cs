namespace Cinema.Models.Entities
{
    public class FilmGenre
    {
        public int FilmId { get; set; }
        public int GenreId { get; set; }
        public FilmEntity Film { get; set; }
        public GenreEntity Genre { get; set; }
    }
}