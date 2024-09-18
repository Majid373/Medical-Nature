using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebSite.Areas.Admin.Models.Users;

namespace WebSite.Areas.Admin.Pages.Users
{
    [Area("Admin")]
    [Authorize(Roles = "مدیر")]
    public class DeleteUserModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public DeleteUserModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public DeleteUserViewModel DeleteUser { get; set; }

        public void OnGet(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;

            DeleteUser = new DeleteUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                FullName = user.FullName,
                RegisterDate = user.RegisterDate
            };
        }

        public IActionResult OnPost()
        {
            var user = _userManager.FindByIdAsync(DeleteUser.Id).Result; 
            
            user.IsDelete = true;

            var result = _userManager.UpdateAsync(user).Result;

            if (result.Succeeded)
            {
                TempData["success"] = "حذف با موفقیت انجام شد";
                return RedirectToPage(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                TempData["error"] = error.Description;
            }

            return Page();
        }

    }
}
