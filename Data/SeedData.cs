using BlogProject.Data;
using BlogProject.Models.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.Data
{
   
    namespace BlogProject
    {
        public static class SeedData
        {
            public static async Task EnsureSeedDataAsync(IServiceProvider services)
            {
                using var scope = services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

                // Admin varsa tekrar seedleme
                if (await userManager.FindByEmailAsync("admin@site.com") != null)
                    return;

                // Roller
                string[] roles = { "Admin", "User" };
                foreach (var role in roles)
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));

                // Kullanıcılar
                var admin = new ApplicationUser
                {
                    UserName = "admin@site.com",
                    Email = "admin@site.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "admin123");
                await userManager.AddToRoleAsync(admin, "Admin");

                var user2 = new ApplicationUser
                {
                    UserName = "user@site.com",
                    Email = "user@site.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user2, "user123");
                await userManager.AddToRoleAsync(user2, "User");

                var commenter = new ApplicationUser
                {
                    UserName = "commenter@site.com",
                    Email = "commenter@site.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(commenter, "user123");
                await userManager.AddToRoleAsync(commenter, "User");

                // Kategoriler
                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(
                        new Category { Name = "Yazılım" },
                        new Category { Name = "Teknoloji" },
                        new Category { Name = "Tasarım" },
                        new Category { Name = "Genel" },
                        new Category { Name = "Eğitim" },
                        new Category { Name = "Seyahat" },
                        new Category { Name = "Sağlık" },
                        new Category { Name = "Spor" },
                        new Category { Name = "Müzik" },
                        new Category { Name = "Sanat" },
                        new Category { Name = "Eğlence" },
                        new Category { Name = "Haberler" },
                        new Category { Name = "Günlük" },
                        new Category { Name = "Kültür" },
                        new Category { Name = "Bilim" }
                    );
                    await context.SaveChangesAsync();
                }

                var categoryId = context.Categories.First().Id;

                // Fotoğraf havuzundan dosyalar
                var imageDir = Path.Combine(env.WebRootPath, "images", "seeds");
                var imageFiles = Directory.Exists(imageDir)
                    ? Directory.GetFiles(imageDir).Where(f => f.EndsWith(".jpg") || f.EndsWith(".png")).ToList()
                    : new List<string>();

                var rand = new Random();

                // Bloglar
                var blogs = new List<BlogPost>();

                for (int i = 1; i <= 5; i++)
                {
                    var adminImage = imageFiles.Count > 0 ? "/images/seeds/" + Path.GetFileName(imageFiles[rand.Next(imageFiles.Count)]) : null;
                    var user2Image = imageFiles.Count > 0 ? "/images/seeds/" + Path.GetFileName(imageFiles[rand.Next(imageFiles.Count)]) : null;

                    blogs.Add(new BlogPost
                    {
                        Title = $"Admin Blog {i}",
                        Content = $"Admin tarafından yazılmış içerik {i}",
                        CategoryId = categoryId,
                        PublishedDate = DateTime.Now.AddDays(-i),
                        UserId = admin.Id,
                        ImageUrl = adminImage
                    });

                    blogs.Add(new BlogPost
                    {
                        Title = $"User2 Blog {i}",
                        Content = $"User2 tarafından yazılmış içerik {i}",
                        CategoryId = categoryId,
                        PublishedDate = DateTime.Now.AddDays(-i - 5),
                        UserId = user2.Id,
                        ImageUrl = user2Image
                    });
                }

                context.BlogPosts.AddRange(blogs);
                await context.SaveChangesAsync();

                // Yorumlar ve yanıtlar
                foreach (var blog in context.BlogPosts.ToList())
                {
                    var comment = new Comment
                    {
                        BlogPostId = blog.Id,
                        UserId = commenter.Id,
                        Content = $"Harika yazı! ({blog.Title})",
                        CreatedAt = DateTime.Now.AddMinutes(-30)
                    };
                    context.Comments.Add(comment);
                    await context.SaveChangesAsync();

                    var reply = new Comment
                    {
                        BlogPostId = blog.Id,
                        ParentId = comment.Id,
                        UserId = blog.UserId,
                        Content = "Teşekkürler!",
                        CreatedAt = DateTime.Now.AddMinutes(-10)
                    };
                    context.Comments.Add(reply);
                }

                await context.SaveChangesAsync();
            }
        }
    }

}
