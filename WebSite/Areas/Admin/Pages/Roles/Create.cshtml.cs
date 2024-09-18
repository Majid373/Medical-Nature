using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebSite.Areas.Admin.Pages.Roles
{
    [Area("Admin")]
    [Authorize(Roles = "مدیر")]
    public class CreateModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [BindProperty]
        public IdentityRole Role { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result =  _roleManager.CreateAsync(Role).Result;
            if (result.Succeeded)
            {
                TempData["success"] = "نقش جدید با موفقیت انجام شد ";
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
