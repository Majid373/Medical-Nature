using Application.Dtos;
using Application.Interfaces.Blogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebSite.Areas.Admin.Pages.Blogs
{
    [Area("Admin")]
    [Authorize(Roles = "مدیر")]
    public class CreateBlogModel : PageModel
    {
        private readonly IBlogService _blogService;

        public CreateBlogModel(IBlogService blogService)
        {
            _blogService = blogService;
        }


        [BindProperty]
        public CreateBlogViewModel createBlog { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _blogService.CreateBlog(createBlog);
            TempData["success"] = " با موفقیت انجام شد";

            return RedirectToPage("/Blogs/BlogIndex");

        }
    }
}
