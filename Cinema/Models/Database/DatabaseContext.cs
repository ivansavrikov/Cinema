using Cinema.Models.Entities;
using Cinema.Models.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Models.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<FilmEntity> Films { get; set; }
        public DbSet<GenreEntity> Genres { get; set; }
        public DbSet<FilmGenreEntity> FilmGenres { get; set; }

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
        }
    }
}
