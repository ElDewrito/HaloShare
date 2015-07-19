using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HaloShare.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<FileDownload> FileDownloads { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<GameMap> GameMaps { get; set; }
        public DbSet<GameMapVariant> GameMapVariants { get; set; }
        public DbSet<GameMapVariantImage> GameMapVariantImages { get; set; }
        public DbSet<GameType> GameTypes { get; set; }
        public DbSet<GameTypeVariant> GameTypeVariants { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Migrations.Configuration>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameMapVariant>()
                .HasMany<GameType>(s => s.SupportedGameTypes)
                .WithMany()
                .Map(mt =>
                {
                    mt.MapLeftKey("GameMapVariantId");
                    mt.MapRightKey("GameTypeId");
                    mt.ToTable("GameMapVariantGameTypes");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}