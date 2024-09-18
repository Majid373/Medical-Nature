using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebSite.Areas.Admin.Pages
{
    [Area("Admin")]
    [Authorize(Roles = "مدیر")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
