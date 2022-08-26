using Detrav.Launcher.Server.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Detrav.Launcher.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<AchievementModel> Achievements { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<BlobModel> Blobs { get; set; }
        public DbSet<FileModel> Files { get; set; }
        public DbSet<ProductUserModel> ProductUsers { get; set; }
        public DbSet<ProductVersionModel> Versions { get; set; }
        public DbSet<ScreenshotModel> Screenshots { get; set; }
        public DbSet<TagModel> Tags { get; set; }
        public DbSet<FileBlobModel> FileBlobs { get; set; }



#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
            : base(options)
        {


        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<FileBlobModel>()
                .HasKey(t => new { t.FileId, t.BlobId });
        }
    }
}