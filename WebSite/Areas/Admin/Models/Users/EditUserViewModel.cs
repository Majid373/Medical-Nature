using System.ComponentModel.DataAnnotations;

namespace WebSite.Areas.Admin.Models.Users
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsDoctor { get; set; }
        public IFormFile? DegreeOfEducationUrl { get; set; }
        public string? DegreeOfEducationName { get; set; }

        public IFormFile? UserAvatarUrl { get; set; }
        public string? UserAvatarName { get; set; }
        public string? Expertise { get; set; }
    }
}
