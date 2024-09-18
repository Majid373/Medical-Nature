using System.ComponentModel.DataAnnotations;

namespace WebSite.Models.ViewModels.User
{
    public class ResetPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string  Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string RePassword { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
