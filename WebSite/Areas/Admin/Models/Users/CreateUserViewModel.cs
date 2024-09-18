using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace WebSite.Areas.Admin.Models.Users
{
    public class CreateUserViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string FullName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string Email { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Password { get; set; }

        public bool IsDoctor { get; set; }

        public IFormFile  UserAvatarUrl { get; set; }
        public string? UserAvatarName { get; set; }

        public IFormFile? DegreeOfEducationUrl { get; set; }
        public string? DegreeOfEducationName { get; set; }

        public string? Expertise { get; set; }

    }
}
