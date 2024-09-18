using Application.Dtos.Roles;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebSite.Areas.Admin.Pages.Roles
{
    [Area("Admin")]
    [Authorize(Roles = "مدیر")]
    public class AddUserRoleModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public AddUserRoleModel(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [BindProperty]
        public AddUserRoleDto AddUserRole { get; set; } = new AddUserRoleDto();

        public void OnGet(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            AddUserRole.Roles = new List<SelectListItem>(
                _roleManager.Roles.Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                }).ToList());

            AddUserRole.Id = id;
            AddUserRole.UserName = user.UserName;

        }

        public IActionResult OnPost()
        {
            User user = _userManager.FindByIdAsync(AddUserRole.Id).Result;

            var result = _userManager.AddToRoleAsync(user, AddUserRole.Role).Result;

            if (result.Succeeded)
            {
                TempData["success"] = " با موفقیت انجام شد";
                return RedirectToPage("/Users/Index");
            }

            foreach (var error in result.Errors)
            {
                TempData["error"] = error.Description;
            }

            return Page();
        }
    }
}
