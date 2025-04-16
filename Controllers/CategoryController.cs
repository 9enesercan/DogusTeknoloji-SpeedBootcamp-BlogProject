using BlogProject.Models.Entities;
using BlogProject.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // List all categories
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return View(categories);
        }

        // View category details
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        // Create a new category
        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.AddAsync(model);
                await _categoryRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // Edit an existing category
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category model)
        {
            if (id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    _categoryRepository.Update(model);
                    await _categoryRepository.SaveAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "An error occurred while updating the category.");
                }
            }

            return View(model);
        }

        // Delete a category
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
