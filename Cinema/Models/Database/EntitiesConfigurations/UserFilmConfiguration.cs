using Cinema.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Models.Database.EntitiesConfigurations
{
    public class UserFilmConfiguration : IEntityTypeConfiguration<UserFilm>
    {
        public void Configure(EntityTypeBuilder<UserFilm> builder)
        {
            builder
                .HasKey(uf => new {uf.UserId, uf.FilmId});

            builder
                .HasOne(uf => uf.Film)
                .WithMany(f => f.UserFilms)
                .HasForeignKey(uf => uf.FilmId);

            builder
                .HasOne(uf => uf.User)
                .WithMany(u => u.UserFilms)
                .HasForeignKey(uf => uf.UserId);
        }
    }
}
