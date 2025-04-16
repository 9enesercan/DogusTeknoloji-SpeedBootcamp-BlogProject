using BlogProject.Models.Entities;
using BlogProject.Models.ViewModels;
using BlogProject.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<BlogPost> _blogRepo;
        private readonly IRepository<Comment> _commentRepo;

        public AdminUserController(UserManager<ApplicationUser> userManager,
                                   IRepository<BlogPost> blogRepo,
                                   IRepository<Comment> commentRepo)
        {
            _userManager = userManager;
            _blogRepo = blogRepo;
            _commentRepo = commentRepo;
        }

        public async Task<IActionResult> Index(string search)
        {
            var users = _userManager.Users.ToList();

            if (!string.IsNullOrWhiteSpace(search))
                users = users.Where(u => u.Email.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            var blogCounts = (await _blogRepo.GetAllAsync()).GroupBy(b => b.UserId)
                                 .ToDictionary(g => g.Key, g => g.Count());

            var commentCounts = (await _commentRepo.GetAllAsync()).GroupBy(c => c.UserId)
                                    .ToDictionary(g => g.Key, g => g.Count());

            var model = users.Select(u => new AdminUserViewModel
            {
                Id = u.Id,
                Email = u.Email,
                Role = _userManager.GetRolesAsync(u).Result.FirstOrDefault() ?? "Yok",
                BlogCount = blogCounts.ContainsKey(u.Id) ? blogCounts[u.Id] : 0,
                CommentCount = commentCounts.ContainsKey(u.Id) ? commentCounts[u.Id] : 0
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);
            await _userManager.AddToRoleAsync(user, role);

            return RedirectToAction("Index");
        }
    }

}
