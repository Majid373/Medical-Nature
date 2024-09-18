using System.ComponentModel.DataAnnotations;

namespace WebSite.Models.ViewModels.User
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "لطفا نام و نام خانوادگی را وارد نمایید")]
        [Display(Name = "نام و نام خانوادگی")]
        [MaxLength(100, ErrorMessage = "نام و نام خانوادگی نباید بیش از 100 کاراکتر باشد")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "لطفا ایمیل را وارد نمایید")]
        [EmailAddress]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا کلمه عبور را وارد نمایید")]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }

        [Required(ErrorMessage = "لطفا تکرار کلمه عبور را وارد نمایید")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "کلمه عبور و تکرار کلمه عبور باید برابر باشد")]
        [Display(Name = "تکرار کلمه عبور")]
        public string RePassword { get; set; }

        public bool IsDoctor { get; set; }



        public IFormFile? ImageUserUrl { get; set; }
        public string? ImageUserName { get; set; } = "avatar.jpg";

        public IFormFile? ImageDegreeOfEducationUrl { get; set; }
        public string? ImageDegreeOfEducationUrlName { get; set; }

    }
}
