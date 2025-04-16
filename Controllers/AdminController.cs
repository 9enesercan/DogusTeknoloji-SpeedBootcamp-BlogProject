using BlogProject.Models.Entities;
using BlogProject.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IRepository<BlogPost> _blogRepo;
        private readonly IRepository<ApplicationUser> _userRepo;
        private readonly IRepository<Comment> _commentRepo;
        private readonly IRepository<Category> _categoryRepo;

        public AdminController(
            IRepository<BlogPost> blogRepo,
            IRepository<ApplicationUser> userRepo,
            IRepository<Comment> commentRepo,
            IRepository<Category> categoryRepo)
        {
            _blogRepo = blogRepo;
            _userRepo = userRepo;
            _commentRepo = commentRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Dashboard()
        {
            var blogs = await _blogRepo.GetAllAsync(b => b.User, b => b.Category);
            var grouped = blogs
                .Where(b => b.User != null)
                .GroupBy(b => b.User)
                .OrderBy(g => g.Key.Email)
                .ToList();

            ViewBag.BlogCount = blogs.Count();
            ViewBag.UserCount = (await _userRepo.GetAllAsync()).Count();
            ViewBag.CommentCount = (await _commentRepo.GetAllAsync()).Count();
            ViewBag.CategoryCount = (await _categoryRepo.GetAllAsync()).Count();

            return View(grouped);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _blogRepo.GetByIdAsync(id);
            if (blog == null)
                return NotFound();

            // Görsel sil
            if (!string.IsNullOrEmpty(blog.ImageUrl))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", blog.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            _blogRepo.Delete(blog);
            await _blogRepo.SaveAsync();

            return RedirectToAction(nameof(Dashboard));
        }
    }
}
