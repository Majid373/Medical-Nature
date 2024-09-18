using WebSite.Areas.Admin.Pages.Users;

namespace WebSite.Models.ViewModels.User
{
    public class UserRequestDto
    {
        public SortType SortType { get; set; }
        public string filterFullName { get; set; }
        public string filterEmail { get; set; }
    }
}
