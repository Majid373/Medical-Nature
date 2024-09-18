using Application.Dtos;
using Application.Interfaces.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebSite.Areas.Admin.Pages.Categories
{
    [Area("Admin")]

    [Authorize(Roles = "مدیر")]
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public IndexModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public List<CategoryViewModel> Category { get; set; }

        public void OnGet()
        {
            Category = _categoryService.GetAllCategory();
        }
    }
}
