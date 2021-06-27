using ACuteArtInterface.Models.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ACuteArtInterface.Models
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }

        public DbSet<UserModel> AccUsers { get; set; }

        public DbSet<ArtistModel> Artists { get; set; }

        public DbSet<AssetModel> Assets { get; set; }

        public DbSet<ArtworkModel> Artworks { get; set; }

        public DbSet<ExhibitionModel> Exhibitions { get; set; }

        public DbSet<ExhibitionArtworkModel> ExhibitionArtworks { get; set; }

        public DbSet<ExhibitionRoomModel> ExhibitionRooms { get; set; }

        public DbSet<ExhibitionRoomItemModel> ExhibitionRoomItems { get; set; }
    }
}
