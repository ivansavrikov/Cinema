using Cinema.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Models.Database.EntitiesConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder
                .HasKey(e => e.Id);

            builder
                .HasMany(u => u.UserFilms)
                .WithOne(uf => uf.User)
                .HasForeignKey(uf => uf.FilmId);
        }
    }
}
