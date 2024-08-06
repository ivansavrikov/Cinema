using System.Collections.Generic;

namespace Cinema.Models.Database.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public List<UserFilm> UserFilms { get; set; }
    }
}
