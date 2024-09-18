namespace WebSite.Areas.Admin.Models.Users
{
    public class DeleteUserViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime RegisterDate { get; set; }


    }
}
