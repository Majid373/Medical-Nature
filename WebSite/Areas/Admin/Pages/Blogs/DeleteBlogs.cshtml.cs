using Application.Dtos;
using Application.Interfaces.Blogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebSite.Areas.Admin.Pages.Blogs
{
    [Area("Admin")]
    [Authorize(Roles = "مدیر")]
    public class DeleteBlogsModel : PageModel
    {
        private readonly IBlogService _blogService;

        public DeleteBlogsModel(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public List<BlogsViewModel> Blog { get; set; }

        public void OnGet()
        {
            Blog = _blogService.GetAllDeletedBlogs();
        }
    }
}
