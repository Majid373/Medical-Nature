using Application.Dtos;
using Application.Interfaces.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebSite.Areas.Admin.Pages.Categories
{
    [Area("Admin")]
    [Authorize(Roles = "مدیر")]
    public class DeleteCategoryModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public DeleteCategoryModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [BindProperty]
        public CategoryViewModel Category { get; set; }

        public void OnGet(int id)
        {
            Category=_categoryService.GetCategory(id);
        }

        public IActionResult OnPost()
        {
            _categoryService.DeleteCategory(Category.Id);
            TempData["success"] = "حذف با موفقیت انجام شد";
            return RedirectToPage(nameof(Index));
        }
    }
}
