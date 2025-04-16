using Microsoft.AspNetCore.Identity;

namespace BlogProject.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<BlogPost>? BlogPosts { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}

