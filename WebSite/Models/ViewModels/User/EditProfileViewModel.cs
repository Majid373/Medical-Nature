namespace WebSite.Models.ViewModels.User
{
    public class EditProfileViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public IFormFile? IamgeUrl { get; set; }
        public string? UserAvatar { get; set; }
        public string? Expertise { get; set; }
        public int PriceTextual { get; set; }
        public int PriceByPhone { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
