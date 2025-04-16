using BlogProject.Models.Entities;
using System.ComponentModel.DataAnnotations;


namespace BlogProject.Models.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int? ParentId { get; set; }
        public Comment? Parent { get; set; }
        public List<Comment>? Replies { get; set; } = new();
    }




}