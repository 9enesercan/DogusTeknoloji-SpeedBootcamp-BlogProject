using BlogProject.Models.Entities;
using BlogProject.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly IRepository<BlogPost> _blogRepo;
    private readonly IRepository<Category> _categoryRepo;

    public HomeController(IRepository<BlogPost> blogRepo, IRepository<Category> categoryRepo)
    {
        _blogRepo = blogRepo;
        _categoryRepo = categoryRepo;
    }

    public async Task<IActionResult> Index(int? categoryId, string? search, int page = 1)
    {
        int pageSize = 6;

        ViewBag.Categories = await _categoryRepo.GetAllAsync();
        ViewBag.SelectedCategory = categoryId;
        ViewBag.Search = search;

        var blogs = await _blogRepo.GetAllAsync(b => b.User, b => b.Category);

        if (!string.IsNullOrEmpty(search))
            blogs = blogs.Where(b => b.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                     b.Content.Contains(search, StringComparison.OrdinalIgnoreCase));

        if (categoryId.HasValue)
            blogs = blogs.Where(b => b.CategoryId == categoryId.Value);

        var totalCount = blogs.Count();
        var pagedBlogs = blogs
            .OrderByDescending(b => b.PublishedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return View(pagedBlogs);
    }
    public IActionResult Privacy()
    {
        return View();
    }

}
