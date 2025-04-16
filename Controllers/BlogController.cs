using BlogProject.Models.Entities;
using BlogProject.Models.ViewModels;
using BlogProject.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogProject.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        private readonly IRepository<BlogPost> _blogRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlogController(IRepository<BlogPost> blogRepository,
                              IRepository<Category> categoryRepository,
                              UserManager<ApplicationUser> userManager)
        {
            _blogRepository = blogRepository;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
        }

        // Blog listeleme
        [AllowAnonymous]
        public async Task<IActionResult> Index(int? categoryId)
        {
            var blogs = await _blogRepository.GetAllAsync(b => b.User, b => b.Category);
            if (categoryId.HasValue)
                blogs = blogs.Where(b => b.CategoryId == categoryId.Value);
            return View(blogs);
        }

        // Yeni blog oluşturma
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            return View();
        }

        // POST: /Blog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogPostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var blogPost = new BlogPost
                {
                    Title = model.Title,
                    Content = model.Content,
                    CategoryId = model.CategoryId,
                    PublishedDate = DateTime.Now,
                    UserId = _userManager.GetUserId(User)
                };

                // Görsel kaydet
                if (model.Image != null && model.Image.Length > 0)
                {
                    var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadsDir)) Directory.CreateDirectory(uploadsDir);

                    var uniqueName = Guid.NewGuid() + Path.GetExtension(model.Image.FileName);
                    var filePath = Path.Combine(uploadsDir, uniqueName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);
                    }

                    blogPost.ImageUrl = "/uploads/" + uniqueName;
                }

                await _blogRepository.AddAsync(blogPost);
                await _blogRepository.SaveAsync();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            return View(model);
        }





        // Blog düzenleme

        public async Task<IActionResult> Edit(int id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            var currentUserId = _userManager.GetUserId(User);

            if (!User.IsInRole("Admin") && blog?.UserId != currentUserId)
                return Unauthorized();

            var model = new BlogPostViewModel
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                CategoryId = blog.CategoryId,
                ImageUrl = blog.ImageUrl
            };

            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", blog.CategoryId);
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BlogPostViewModel model, IFormFile? ImageFile)
        {
            var existing = await _blogRepository.GetByIdAsync(model.Id);
            if (existing == null || existing.UserId != _userManager.GetUserId(User))
                return Unauthorized();

            if (ModelState.IsValid)
            {
                existing.Title = model.Title;
                existing.Content = model.Content;
                existing.CategoryId = model.CategoryId;

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    if (!string.IsNullOrEmpty(existing.ImageUrl))
                    {
                        var oldPath = Path.Combine("wwwroot", existing.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    var ext = Path.GetExtension(ImageFile.FileName);
                    var newFileName = $"{Guid.NewGuid()}{ext}";
                    var path = Path.Combine("wwwroot/uploads", newFileName);
                    using var stream = new FileStream(path, FileMode.Create);
                    await ImageFile.CopyToAsync(stream);
                    existing.ImageUrl = $"/uploads/{newFileName}";
                }

                _blogRepository.Update(existing);
                await _blogRepository.SaveAsync();

                return RedirectToAction("MyBlogs");
            }

            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", model.CategoryId);
            return View(model);
        }





        // Blog silme


        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null || blog.UserId != _userManager.GetUserId(User))
                return Unauthorized();

            _blogRepository.Delete(blog);
            await _blogRepository.SaveAsync();
            return RedirectToAction("Index");
        }
        

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogPost = await _blogRepository.GetByIdAsync(id);
            if (blogPost == null)
                return NotFound();

            _blogRepository.Delete(blogPost);
            await _blogRepository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> MyBlogs()
        {
            var userId = _userManager.GetUserId(User);
            var myBlogs = await _blogRepository.GetAllAsync(b => b.Category);
            return View(myBlogs.Where(b => b.UserId == userId));
        }



        public ICollection<Comment>? Comments { get; set; }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var blog = await _blogRepository.GetByIdAsync(
                id,
                b => b.User,
                b => b.Category,
                b => b.Comments 
            );

            if (blog == null)
                return NotFound();

            
            foreach (var comment in blog.Comments ?? new List<Comment>())
            {
                comment.User = await _userManager.FindByIdAsync(comment.UserId);
            }

            return View(blog);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Json(new { location = "/uploads/" + fileName });



            }

            return BadRequest();
        }



    }

}
