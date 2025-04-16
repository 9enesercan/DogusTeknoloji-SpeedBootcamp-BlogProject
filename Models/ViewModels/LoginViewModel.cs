using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email gerekli")]
        [EmailAddress(ErrorMessage = "Geçerli bir email giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre gerekli")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

}
