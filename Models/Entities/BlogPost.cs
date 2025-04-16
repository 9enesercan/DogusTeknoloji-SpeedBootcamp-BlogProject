using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models.Entities
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "İçerik zorunludur.")]
        public string Content { get; set; }

        
        public int? CategoryId { get; set; }

        [Required]
        [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever] 
        public string UserId { get; set; }


        public DateTime PublishedDate { get; set; }

        public string? ImageUrl { get; set; }  // Eksik olan alan (opsiyonel görsel yolu)

            
        public Category? Category { get; set; }
        public ApplicationUser? User { get; set; }

        public ICollection<Comment>? Comments { get; set; } // Eksik olan yorum ilişkisi
    }
}
