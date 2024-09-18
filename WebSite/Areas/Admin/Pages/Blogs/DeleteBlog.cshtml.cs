using Application.Dtos;
using Application.Interfaces.Blogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebSite.Areas.Admin.Pages.Blogs
{
    [Area("Admin")]
    [Authorize(Roles = "مدیر")]
    public class DeleteBlogModel : PageModel
    {
        private readonly IBlogService _blogService;

        public DeleteBlogModel(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public BlogsViewModel Blog { get; set; }

        public void OnGet(int id)
        {
            Blog = _blogService.GetBlog(id);
        }

        public IActionResult OnPost(int id)
        {
            _blogService.DeleteBlog(id);

            TempData["success"] = "حذف با موفقیت انجام شد";

            return RedirectToPage("/Blogs/BlogIndex");
        }
    }
}
