using Application.Services;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebSite.Areas.Admin.Models.Users;

namespace WebSite.Areas.Admin.Pages.Users
{
    [Area("Admin")]
    [Authorize(Roles = "مدیر")]
    public class CreateUserModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly EmailService _emailService;

        public CreateUserModel(UserManager<User> userManager)
        {
            _userManager = userManager;
            _emailService = new EmailService();
        }

        [BindProperty]
        public CreateUserViewModel CreateUser { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (CreateUser.UserAvatarUrl != null)
            {
                string imagePath = "";

                CreateUser.UserAvatarName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(CreateUser.UserAvatarUrl.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", CreateUser.UserAvatarName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    CreateUser.UserAvatarUrl.CopyTo(stream);
                }
            }


            if (CreateUser.DegreeOfEducationUrl != null)
            {
                string imagePath = "";

                CreateUser.DegreeOfEducationName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(CreateUser.DegreeOfEducationUrl.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/DegreeOfEducation", CreateUser.DegreeOfEducationName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    CreateUser.DegreeOfEducationUrl.CopyTo(stream);
                }
            }

            User newUser = new User()
            {
                Email = CreateUser.Email,
                UserName = CreateUser.Email,
                FullName = CreateUser.FullName,
                RegisterDate = DateTime.Now,
                IsDoctor = CreateUser.IsDoctor,
                ImageName=CreateUser.UserAvatarName,
                DegreeOfEducation=CreateUser.DegreeOfEducationName,
                Expertise=CreateUser.Expertise,
            };

            var result = _userManager.CreateAsync(newUser, CreateUser.Password).Result;

            if (result.Succeeded)
            {
                var token = _userManager.GenerateEmailConfirmationTokenAsync(newUser).Result;

                string callbackUrl = Url.Action("ConfirmEmail", "Account", new
                {
                    UserId = newUser.Id,
                    token = token
                }, protocol: Request.Scheme);

                string body = $"لطفا برای فعالسازی حساب کاربری بر روی لینک زیر کلیک کنید!  <br/> <a href={callbackUrl}> Link </a>";
                _emailService.Execute(newUser.Email, body, "فعالسازی حساب کاربری");

                TempData["success"] = "ثبت نام با موفقیت انجام شد";

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
