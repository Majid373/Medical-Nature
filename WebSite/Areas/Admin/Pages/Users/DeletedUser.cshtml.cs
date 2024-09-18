using Application.Services;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebSite.Areas.Admin.Pages.Users
{
    [Area("Admin")]
    [Authorize(Roles = "مدیر")]
    public class DeletedUserModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public DeletedUserModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public List<User> Users { get; set; }

        public void OnGet(int? pageNumber)
        {

            var pageSize = 5;
            Users = PaginatedList<User>.Create(_userManager.Users.Where(u => u.IsDelete).ToList(), pageNumber ?? 1, pageSize);
        }
    }
}
