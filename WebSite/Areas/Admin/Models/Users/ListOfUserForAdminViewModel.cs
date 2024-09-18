namespace WebSite.Areas.Admin.Models.Users
{
    public class ListOfUserForAdminViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsDelete { get; set; }
        public DateTime RegisterDate { get; set; }
        public string RoleName { get; set; }
    }
}
