using BlogProject.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // FK ayarları
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.BlogPost)
                .WithMany(b => b.Comments)
                .HasForeignKey(c => c.BlogPostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
                .HasMany(c => c.Replies)
                .WithOne(c => c.Parent)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.NoAction); 


            // Sabit kategoriler
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Genel" },
                new Category { Id = 2, Name = "Teknoloji" },
                new Category { Id = 3, Name = "Yazılım" },
                new Category { Id = 4, Name = "Günlük Hayat" }
            );

            // SABİT GUID kullanarak rolleri seed et
            var adminRoleId = "A1B2C3D4-ADMIN-0001-0000-000000000001";
            var userRoleId = "B2C3D4E5-USER-0001-0000-000000000002";

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = adminRoleId // zorunlu hale geldi EF7+
                },
                new IdentityRole
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = userRoleId
                }
            );
        }
    }
}
