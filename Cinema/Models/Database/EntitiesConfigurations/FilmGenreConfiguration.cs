using Cinema.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Models.EntityConfigurations
{
    public class FilmGenreConfiguration : IEntityTypeConfiguration<FilmGenreEntity>
    {
        public void Configure(EntityTypeBuilder<FilmGenreEntity> builder)
        {
            builder.HasKey(fg => new { fg.FilmId, fg.GenreId });

            builder.HasOne(fg => fg.Film)
                .WithMany(f => f.FilmGenres)
                .HasForeignKey(fg => fg.FilmId);

            builder.HasOne(fg => fg.Genre)
                .WithMany(g => g.FilmGenres)
                .HasForeignKey(fg => fg.GenreId);
        }
    }

}
