using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebSite.Areas.Admin.Pages.Users
{
    [Area("Admin")]
    [Authorize(Roles = "مدیر")]
    public class DoctorDetailModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public DoctorDetailModel(UserManager<User> userManager)
        {
            _userManager = userManager; 
        }

        public User User { get; set; }

        public void OnGet(string id)
        {
            User=_userManager.FindByIdAsync(id).Result;
        }
    }
}
