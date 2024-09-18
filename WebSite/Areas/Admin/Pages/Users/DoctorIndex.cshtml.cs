using Application.Services;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebSite.Models.ViewModels.User;

namespace WebSite.Areas.Admin.Pages.Users
{
    [Area("Admin")]
    [Authorize(Roles = "مدیر")]
    public class DoctorIndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public DoctorIndexModel(UserManager<User>  userManager)
        {
            _userManager = userManager;
        }

        public PaginatedList<User> Users { get; set; }

        public void OnGet(UserRequestDto request, int? pageNumber)
        {
            var query = _userManager.Users.AsQueryable();
            if (!string.IsNullOrEmpty(request.filterFullName))
            {
                query = query.Where(u => u.FullName.Contains(request.filterFullName));
            }

            if (!string.IsNullOrEmpty(request.filterEmail))
            {
                query = query.Where(u => u.FullName.Contains(request.filterEmail));
            }

            var pageSize = 5;
            Users = PaginatedList<User>.Create(query.Where(u => (!u.IsDelete && u.DegreeOfEducation != null) || (u.IsDoctor && !u.IsDelete)).ToList(), pageNumber ?? 1, pageSize);
        }
    }
}
