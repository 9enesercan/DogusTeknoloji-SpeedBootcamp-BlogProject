using BlogProject.Models.Entities;
using BlogProject.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminCategoryController : Controller
    {
        private readonly IRepository<Category> _categoryRepo;
        private readonly IRepository<BlogPost> _blogRepo;

        public AdminCategoryController(IRepository<Category> categoryRepo, IRepository<BlogPost> blogRepo)
        {
            _categoryRepo = categoryRepo;
            _blogRepo = blogRepo;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var newCategory = new Category { Name = name.Trim() };
                await _categoryRepo.AddAsync(newCategory);
                await _categoryRepo.SaveAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null) return NotFound();

            var relatedBlogs = (await _blogRepo.GetAllAsync()).Any(b => b.CategoryId == id);
            if (relatedBlogs)
            {
                TempData["Error"] = "Bu kategoriye bağlı blog yazıları olduğu için silinemez.";
                return RedirectToAction(nameof(Index));
            }

            _categoryRepo.Delete(category);
            await _categoryRepo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
