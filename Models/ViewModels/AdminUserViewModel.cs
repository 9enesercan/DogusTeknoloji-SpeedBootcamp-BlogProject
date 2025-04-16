namespace BlogProject.Models.ViewModels
{
    public class AdminUserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int BlogCount { get; set; }
        public int CommentCount { get; set; }
    }
}

