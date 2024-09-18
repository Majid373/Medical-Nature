using Application.Dtos;
using Domain.Users;

namespace WebSite.Models.ViewModels.Index
{
    public class IndexViewModel
    {
        public List<CategoryViewModel> Categories { get; set; }
        public List<Domain.Users.User> Users { get; set; }
        public List<BlogsForWebSiteViewModel> Blogs { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
