using Cinema.Models.Entities;

namespace Cinema.Models.Database.Entities
{
    public class UserFilm
    {
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        public int FilmId { get; set; }
        public FilmEntity Film { get; set; }
    }
}