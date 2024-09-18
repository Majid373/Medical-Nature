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
    public class UsersInRoleModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public UsersInRoleModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public List<ListOfUserForAdminViewModel> Users { get; set; }

        public void OnGet(string name)
        {
            var userInRole = _userManager.GetUsersInRoleAsync(name).Result;

            Users = userInRole.Select(u => new ListOfUserForAdminViewModel
            {
                Email = u.Email,
                EmailConfirmed = u.EmailConfirmed,
                FullName = u.FullName,
                RegisterDate = u.RegisterDate,
                Id = u.Id,
                IsDelete = u.IsDelete,
                RoleName=name,
                
            }).ToList();    

        }
    }
}
