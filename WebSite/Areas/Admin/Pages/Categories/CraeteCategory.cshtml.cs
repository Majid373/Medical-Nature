using Application.Dtos;
using Application.Interfaces.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebSite.Areas.Admin.Pages.Categories
{
    [Area("Admin")]
    [Authorize(Roles = "مدیر")]
    public class CraeteCategoryModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public CraeteCategoryModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [BindProperty]
        public CategoryViewModel category { get; set; }

        public void OnGet()
        {
          
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _categoryService.CreateCategory(category);

            TempData["success"] = " موفقیت انجام شد";
            return RedirectToPage(nameof(Index));
        }
    }
}
