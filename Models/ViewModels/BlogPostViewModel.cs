using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models.ViewModels
{
    public class BlogPostViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public int? CategoryId { get; set; }

        public string? ImageUrl { get; set; } // mevcut görsel yolu

        public IFormFile? Image { get; set; } // yeni yüklenen dosya (isteğe bağlı)
    }


}
