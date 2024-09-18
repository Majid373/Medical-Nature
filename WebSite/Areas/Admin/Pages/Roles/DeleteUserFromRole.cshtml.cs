using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebSite.Areas.Admin.Models.Users;

namespace WebSite.Areas.Admin.Pages.Roles
{
    [Area("Admin")]
    [Authorize(Roles = "مدیر")]
    public class DeleteUserFromRoleModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public DeleteUserFromRoleModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public ListOfUserForAdminViewModel User { get; set; }

        public void OnGet(string id, string roleName)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            User = new ListOfUserForAdminViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                EmailConfirmed = user.EmailConfirmed,
                IsDelete = user.IsDelete,
                RoleName = roleName,
                RegisterDate = user.RegisterDate,
            };
        }

        public IActionResult OnPost(string roleName)
        {
            var user = _userManager.FindByIdAsync(User.Id).Result;

            var result = _userManager.RemoveFromRoleAsync(user, roleName).Result;

            if (result.Succeeded)
            {
                TempData["success"] = " با موفقیت انجام شد";
                return RedirectToPage("/Roles/UsersInRole", new { name = roleName });
            }

            foreach (var error in result.Errors)
            {
                TempData["error"] =error.Description;
            }

            return Page();
        }
    }
}
