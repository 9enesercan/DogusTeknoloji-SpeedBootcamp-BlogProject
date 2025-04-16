using BlogProject.Models.Entities;
using BlogProject.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers
{
    [Authorize]
    
    public class CommentController : Controller
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentController(IRepository<Comment> commentRepository, UserManager<ApplicationUser> userManager)
        {
            _commentRepository = commentRepository;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int blogPostId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["Error"] = "Yorum boş olamaz.";
                return RedirectToAction("Details", "Blog", new { id = blogPostId });
            }

            var comment = new Comment
            {
                BlogPostId = blogPostId,
                Content = content,
                CreatedAt = DateTime.Now,
                UserId = _userManager.GetUserId(User)
            };

            await _commentRepository.AddAsync(comment);
            await _commentRepository.SaveAsync();

            return RedirectToAction("Details", "Blog", new { id = blogPostId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null || comment.UserId != _userManager.GetUserId(User))
                return Unauthorized();

            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Comment updated)
        {
            var existing = await _commentRepository.GetByIdAsync(updated.Id);
            if (existing == null || existing.UserId != _userManager.GetUserId(User))
                return Unauthorized();

            existing.Content = updated.Content;
            await _commentRepository.SaveAsync();

            return RedirectToAction("Details", "Blog", new { id = existing.BlogPostId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Comment comment)
        {
            if (string.IsNullOrWhiteSpace(comment.Content))
                return RedirectToAction("Details", "Blog", new { id = comment.BlogPostId });

            comment.UserId = _userManager.GetUserId(User);
            comment.CreatedAt = DateTime.Now;

            await _commentRepository.AddAsync(comment);
            await _commentRepository.SaveAsync();

            return RedirectToAction("Details", "Blog", new { id = comment.BlogPostId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
                return NotFound();

            // sadece yorumu yazan silebilir
            if (comment.UserId != _userManager.GetUserId(User))
                return Forbid();

            var blogId = comment.BlogPostId;

            _commentRepository.Delete(comment);
            await _commentRepository.SaveAsync();

            return RedirectToAction("Details", "Blog", new { id = blogId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(Comment model)
        {
            if (string.IsNullOrWhiteSpace(model.Content))
                return RedirectToAction("Details", "Blog", new { id = model.BlogPostId });

            model.CreatedAt = DateTime.Now;
            model.UserId = _userManager.GetUserId(User);

            await _commentRepository.AddAsync(model);
            await _commentRepository.SaveAsync();

            return RedirectToAction("Details", "Blog", new { id = model.BlogPostId });
        }


    }

}


