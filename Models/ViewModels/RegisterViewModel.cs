using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email gerekli")]
        [EmailAddress(ErrorMessage = "Geçerli bir email giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre gerekli")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}

