using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebSite.Areas.Admin.Pages.Roles
{
    [Area("Admin")]
    [Authorize(Roles = "مدیر")]
    public class DeleteModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public DeleteModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [BindProperty]
        public IdentityRole Role { get; set; }

        public void OnGet(string id)
        {
            Role = _roleManager.FindByIdAsync(id).Result;
        }

        public IActionResult OnPost()
        {
            var result = _roleManager.DeleteAsync(Role).Result;

            if (result.Succeeded)
            {
                TempData["success"] = "عملیات با موفقیت انجام شد ";
                return RedirectToPage("/Roles/Index");
            }

            foreach (var error in result.Errors)
            {
                TempData["error"] = error.Description;
            }
            return Page();
        }
    }
}
