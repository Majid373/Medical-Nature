using Application.Dtos;
using Application.Interfaces.Blogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebSite.Areas.Admin.Pages.Blogs
{
    [Area("Admin")]
    [Authorize(Roles = "مدیر")]
    public class EditBlogModel : PageModel
    {
        private readonly IBlogService _blogService;

        public EditBlogModel(IBlogService blogService)
        {
            _blogService = blogService;
        }


        [BindProperty]
        public EditBlogViewModel EditBlog { get; set; }

        public void OnGet(int id)
        {
            EditBlog = _blogService.GetBlogForEdit(id);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _blogService.EditBlog(EditBlog);
            TempData["success"] = "ویرایش با موفقیت انجام شد";
            return RedirectToPage("/Blogs/BlogIndex");
        }
    }
}
