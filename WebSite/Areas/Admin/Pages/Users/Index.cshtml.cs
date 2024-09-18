
using Application.Services;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;
using WebSite.Areas.Admin.Models.Users;
using WebSite.Models.ViewModels.User;

namespace WebSite.Areas.Admin.Pages.Users
{
    [Area("Admin")]
    [Authorize(Roles = "مدیر")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public IndexModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public PaginatedList<User> Users { get; set; }

        public void OnGet(UserRequestDto request , int? pageNumber)
        {
            var query = _userManager.Users.AsQueryable();

            if (request.SortType == SortType.User)
            {
                query = query.Where(u => !u.IsDelete && !u.IsDoctor && u.DegreeOfEducation == null);
            }

            if (request.SortType == SortType.Doctor)
            {
                query = query.Where(u => !u.IsDelete && (u.IsDoctor || u.DegreeOfEducation!=null));
            }

            if (request.SortType == SortType.ConfirmedUser)
            {
                query = query.Where(u => !u.IsDelete && !u.IsDoctor && u.DegreeOfEducation == null && u.EmailConfirmed);
            }

            if (request.SortType == SortType.ConfirmedDoctor)
            {
                query = query.Where(u => !u.IsDelete && u.IsDoctor);
            }

            if (request.SortType == SortType.DeletedUser)
            {
                query = query.Where(u => u.IsDelete && !u.IsDoctor && u.DegreeOfEducation == null);
            }

            if (request.SortType == SortType.DeletedDoctor)
            {
                query = query.Where(u => u.IsDelete && u.IsDoctor && u.DegreeOfEducation == null);
            }

            if (request.SortType == SortType.UnverifiedDoctors)
            {
                query = query.Where(u => !u.IsDelete && !u.IsDoctor && u.DegreeOfEducation != null);
            }

            if(!string.IsNullOrEmpty(request.filterFullName))
            {
                query = query.Where(u => u.FullName.Contains(request.filterFullName));
            }

            if (!string.IsNullOrEmpty(request.filterEmail))
            {
                query = query.Where(u => u.FullName.Contains(request.filterEmail));
            }

            var pageSize = 5;
            Users = PaginatedList<User>.Create(query.ToList(), pageNumber ?? 1, pageSize);

        }
    }
    public enum SortType
    {
        None = 0,
        User = 1,
        Doctor = 2,
        ConfirmedUser = 3,
        ConfirmedDoctor = 4,
        DeletedUser = 5,
        DeletedDoctor = 6,
        UnverifiedDoctors = 7,
    }
}
