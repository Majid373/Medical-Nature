using Application.Dtos;
using Application.Interfaces.Blogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebSite.Areas.Admin.Pages.Blogs
{
    [Area("Admin")]
    [Authorize(Roles = "مدیر")]
    public class BlogIndexModel : PageModel
    {
        private readonly IBlogService _blogService;

        public BlogIndexModel(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public List<BlogsViewModel> Blogs { get; set; }

        public void OnGet()
        {
            Blogs = _blogService.GetAllBlogs();
        }
    }
}
