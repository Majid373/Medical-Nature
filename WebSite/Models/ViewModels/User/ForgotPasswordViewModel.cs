 using System.ComponentModel.DataAnnotations;

namespace WebSite.Models.ViewModels.User
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "لطفا ایمیل خود را وارد نمایید")]
        [Display(Name = "ایمیل")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
