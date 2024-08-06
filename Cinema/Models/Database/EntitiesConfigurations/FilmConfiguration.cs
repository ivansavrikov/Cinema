using Cinema.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Models.EntityConfigurations
{
    public class FilmConfiguration : IEntityTypeConfiguration<FilmEntity>
    {
        public void Configure(EntityTypeBuilder<FilmEntity> builder)
        {
            builder
                .HasKey(e => e.Id);

            builder
                .HasIndex(i => i.KinopoiskId)
                .IsUnique();

            builder.HasMany(f => f.FilmGenres)
                .WithOne(fg => fg.Film)
                .HasForeignKey(fg => fg.FilmId);

            builder
                .HasMany(f => f.UserFilms)
                .WithOne(uf => uf.Film)
                .HasForeignKey(uf => uf.FilmId);
        }
    }
}
