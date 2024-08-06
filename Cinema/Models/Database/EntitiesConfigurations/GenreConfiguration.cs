using Cinema.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Models.EntityConfigurations
{
    public class GenreConfiguration : IEntityTypeConfiguration<GenreEntity>
    {
        public void Configure(EntityTypeBuilder<GenreEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasMany(g => g.FilmGenres)
                .WithOne(fg => fg.Genre)
                .HasForeignKey(fg => fg.GenreId);
        }
    }
}