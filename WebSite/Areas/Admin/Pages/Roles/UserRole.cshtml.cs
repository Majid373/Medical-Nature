using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebSite.Areas.Admin.Pages.Roles
{
    [Area("Admin")]
    [Authorize(Roles = "مدیر")]
    public class UserRoleModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public UserRoleModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public IList<string> Roles { get; set; }

        public void OnGet(string id)
        {
            User user = _userManager.FindByIdAsync(id).Result;

            Roles = _userManager.GetRolesAsync(user).Result;
        }
    }
}
