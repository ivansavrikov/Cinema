using Cinema.Models.Database.Entities;
using Cinema.Models.Database.EntitiesConfigurations;
using Cinema.Models.Entities;
using Cinema.Models.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Models.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserFilm> UserFilms { get; set; }
        public DbSet<FilmEntity> Films { get; set; }
        public DbSet<GenreEntity> Genres { get; set; }
        public DbSet<FilmGenre> FilmGenres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=films.db");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FilmConfiguration());
            modelBuilder.ApplyConfiguration(new GenreConfiguration());
            modelBuilder.ApplyConfiguration(new FilmGenreConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserFilmConfiguration());
        }
    }
}
