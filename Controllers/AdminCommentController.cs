using BlogProject.Models.Entities;
using BlogProject.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BlogProject.Controllers
{
    [Authorize(Roles = "Admin")]
public class AdminCommentController : Controller
{
    private readonly IRepository<Comment> _commentRepository;
    private readonly IRepository<BlogPost> _blogRepository;

    public AdminCommentController(IRepository<Comment> commentRepo, IRepository<BlogPost> blogRepo)
    {
        _commentRepository = commentRepo;
        _blogRepository = blogRepo;
    }

    public async Task<IActionResult> Index()
    {
        var comments = await _commentRepository.GetAllAsync(c => c.User, c => c.BlogPost);
        var grouped = comments
            .GroupBy(c => c.BlogPost)
            .OrderByDescending(g => g.Key.PublishedDate)
            .ToList();

        return View(grouped);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment == null) return NotFound();

        _commentRepository.Delete(comment);
        await _commentRepository.SaveAsync();

        return RedirectToAction("Index");
    }
}
}
