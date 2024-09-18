using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebSite.Areas.Admin.Models.Users;

namespace WebSite.Areas.Admin.Pages.Users
{
    [Area("Admin")]
    [Authorize(Roles = "مدیر")]
    public class EditUserModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public EditUserModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public EditUserViewModel EditUser { get; set; }

        public void OnGet(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;

            EditUser = new EditUserViewModel()
            {
                Email = user.Email,
                FullName = user.FullName,
                Id = user.Id,
                IsDoctor = user.IsDoctor,
                DegreeOfEducationName = user.DegreeOfEducation,
                UserAvatarName = user.ImageName,
                Expertise= user.Expertise,

            };

        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (EditUser.UserAvatarUrl != null)
            {
                string imagePath = "";

                //if (EditUser.UserAvatarName != "avatar.jpg")
                //{
                //    imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", EditUser.UserAvatarName);
                //    if (System.IO.File.Exists(imagePath))
                //    {
                //        System.IO.File.Delete(imagePath);
                //    }
                //}

                EditUser.UserAvatarName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(EditUser.UserAvatarUrl.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", EditUser.UserAvatarName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    EditUser.UserAvatarUrl.CopyTo(stream);
                }

            }

            if (EditUser.DegreeOfEducationUrl != null)
            {
                string imagePath = "";

             

                EditUser.DegreeOfEducationName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(EditUser.DegreeOfEducationUrl.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/DegreeOfEducation", EditUser.DegreeOfEducationName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    EditUser.DegreeOfEducationUrl.CopyTo(stream);
                }

            }


            var user = _userManager.FindByIdAsync(EditUser.Id).Result; 
            user.FullName= EditUser.FullName;
            user.IsDoctor= EditUser.IsDoctor;

            user.ImageName = EditUser.UserAvatarName;
            user.DegreeOfEducation=EditUser.DegreeOfEducationName;

            var result = _userManager.UpdateAsync(user).Result;

            if (result.Succeeded)
            {
                TempData["success"] = "ویرایش با موفقیت انجام شد";
                return RedirectToPage("Index");
            }

            foreach (var error in result.Errors)
            {
                TempData["error"] = error.Description;
            }
                return Page();
        }
    }
}
