using Application.Interfaces.Appointments;
using Application.Services;
using Domain.Appointments;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using NuGet.Common;
using System.IO;
using WebSite.Models.ViewModels.User;

namespace WebSite.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Domain.Users.User> _userManager;
        private readonly SignInManager<Domain.Users.User> _signInManager;
        private readonly EmailService _emailService;
        private readonly IAppointmentService _appointmentService;

        public AccountController(UserManager<Domain.Users.User> userManager, SignInManager<User> signInManager,
            IAppointmentService appointmentService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = new EmailService();
            _appointmentService = appointmentService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            if (register.ImageUserUrl != null)
            {
                string imagePath = "";

                register.ImageUserName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(register.ImageUserUrl.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", register.ImageUserName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    register.ImageUserUrl.CopyTo(stream);
                }
            }

            if (register.ImageDegreeOfEducationUrl != null)
            {
                string imagePath = "";

                register.ImageDegreeOfEducationUrlName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(register.ImageDegreeOfEducationUrl.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/DegreeOfEducation", register.ImageDegreeOfEducationUrlName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    register.ImageDegreeOfEducationUrl.CopyTo(stream);
                }
            }


            User newUser = new User()
            {
                Email = register.Email,
                FullName = register.FullName,
                UserName = register.Email,
                RegisterDate = DateTime.Now,
                IsDelete = false,
                IsDoctor = register.IsDoctor,
                DegreeOfEducation = register.ImageDegreeOfEducationUrlName,
                ImageName = register.ImageUserName,

            };

            var result = _userManager.CreateAsync(newUser, register.Password).Result;

            if (result.Succeeded)
            {
                var token = _userManager.GenerateEmailConfirmationTokenAsync(newUser).Result;
                string callbackUrl = Url.Action("ConfirmEmail", "Account", new
                {
                    UserId = newUser.Id,
                    token = token
                }, protocol: Request.Scheme);

                string body = $"لطفا برای فعالسازی حساب کاربری بر روی لینک زیر کلیک کنید!  <br/> <a href={callbackUrl}> Link </a>";
                _emailService.Execute(newUser.Email, body, "فعال سازی حساب کاربری");

                TempData["success"] = "ثبت نام با موفقیت انجام شد";
                return RedirectToAction(nameof(Login));
            }

            foreach (var error in result.Errors)
            {
                TempData["error"] = error.Description;
            }

            return View(register);
        }

        public IActionResult ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return BadRequest();
            }
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null)
            {
                return View("Error");
            }

            var result = _userManager.ConfirmEmailAsync(user, token).Result;
            if (result.Succeeded)
            {
                TempData["success"] = "حساب کاربری با موفقیت فعال شد ";
                return RedirectToAction("login");
            }
            else
            {
                TempData["error"] = "عملیات با شکست مواجه شد ";
            }
            return RedirectToAction("login");
        }

        public IActionResult Login(string returnUrl = "/")
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl,
            });
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _userManager.FindByNameAsync(login.Email).Result;

            if (user == null)
            {
                TempData["error"] = "کاربری با مشخصات وارد شده یافت نشد";
                return View(login);
            }

            _signInManager.SignOutAsync();

            var result = _signInManager.PasswordSignInAsync(user, login.Password, login.IsPersistent, true).Result;

            if (result.Succeeded)
            {
                if (login.ReturnUrl == "/")
                {
                    TempData["success"] = "ورود با موفقیت انجام شد";

                    return RedirectToAction(nameof(Profile));
                }
                else
                {
                    return Redirect(login.ReturnUrl);
                }
            }

            if (result.RequiresTwoFactor)
            {

            }

            if (result.IsLockedOut)
            {

            }

            TempData["error"] = "عملیات با شکست مواجه شد ";

            return View(login);

        }

        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            TempData["success"] = "  خروج با موفقیت انجام شد";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(forgotPassword);
            }

            var user = _userManager.FindByEmailAsync(forgotPassword.Email).Result;
            if (user == null || _userManager.IsEmailConfirmedAsync(user).Result == false)
            {
                return View("Error");
            }

            string token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
            string callbackUrl = Url.Action("ResetPassword", "Account", new
            {
                userId = user.Id,
                token = token
            }, protocol: Request.Scheme);

            string body = $"برای تنظیم مجدد کلمه عبور بر روی لینک زیر کلیک کنید <br/> <a href={callbackUrl}>Reset Password</a>";
            _emailService.Execute(user.Email, body, "بازیابی کلمه عبور");
            TempData["info"] = "لینک بازیابی کلمه عبور برای ایمیل شما ارسال شد ";
            return View();
        }

        public IActionResult ResetPassword(string userId, string token)
        {
            return View(new ResetPasswordViewModel
            {
                UserId = userId,
                Token = token
            });

        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPassword);
            }

            if (resetPassword.Password != resetPassword.RePassword)
            {
                return BadRequest();
            }

            var user = _userManager.FindByIdAsync(resetPassword.UserId).Result;
            if (user == null)
            {
                return BadRequest();
            }

            var result = _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password).Result;

            if (result.Succeeded)
            {
                TempData["success"] = " موفقیت انجام شد";
                return RedirectToAction(nameof(Login));
            }


            foreach (var error in result.Errors)
            {
                TempData["error"] = error.Description;
            }
            return View();

        }

        public IActionResult ResetPasswordConfimation()
        {
            return View();
        }


        [Authorize]
        public IActionResult Profile()
        {
            var user = _userManager.GetUserAsync(User).Result;

            var data = new ProfileViewModel()
            {
                FullName = user.FullName,
                Email = user.Email,
                RegisterDate = user.RegisterDate,
                ImageName = user.ImageName,
            };

            return View(data);
        }

        [Authorize]
        public IActionResult EditProfile()
        {
            var user = _userManager?.GetUserAsync(User).Result;

            var data = new EditProfileViewModel()
            {
                Id = user.Id,
                FullName = user.FullName,
                UserAvatar = user.ImageName,
                Expertise = user.Expertise,
                PriceByPhone = user.PriceByPhone,
                PriceTextual = user.PriceTextual,

            };

            return View(data);
        }

        [HttpPost]
        public IActionResult EditProfile(EditProfileViewModel editProfile)
        {
            if (!ModelState.IsValid)
            {
                return View(editProfile);
            }

            if (editProfile.IamgeUrl != null)
            {
                string imagePath = "";

                if (editProfile.UserAvatar != "avatar.jpg")
                {
                    imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", editProfile.UserAvatar);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                editProfile.UserAvatar = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(editProfile.IamgeUrl.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", editProfile.UserAvatar);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    editProfile.IamgeUrl.CopyTo(stream);
                }

            }


            var user = _userManager.GetUserAsync(User).Result;
            user.FullName = editProfile.FullName;
            user.ImageName = editProfile.UserAvatar;
            user.Expertise = editProfile.Expertise;
            user.PriceByPhone = editProfile.PriceByPhone;
            user.PriceTextual = editProfile.PriceTextual;

            var result = _userManager.UpdateAsync(user).Result;

            if (result.Succeeded)
            {
                TempData["success"] = "ورایش حساب کاربری با موفقیت انجام شد";
                return RedirectToAction("Profile");
            }

            foreach (var error in result.Errors)
            {
                TempData["error"] = error.Description;
            }

            return View();
        }

        [Authorize]
        public IActionResult Appointment()
        {
            var data = _appointmentService.GetAllAppointments(_userManager.GetUserId(User));

            return View(data);
        }

        [Authorize]

        public IActionResult AddAppointment()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddAppointment(Appointment appointment)
        {

            appointment.IsReserved = false;
            appointment.UserId = _userManager.GetUserId(User);

            _appointmentService.AddAppointment(appointment);
            TempData["success"] = " با موفقیت انجام شد";

            return RedirectToAction(nameof(Appointment));
        }


        public IActionResult EditAppointment(int id)
        {
            var data = _appointmentService.GetAppointmentById(id);

            return View(data);
        }

        [HttpPost]
        public IActionResult EditAppointment(Appointment appointment)
        {
            _appointmentService.UpdateAppointment(appointment);
            TempData["success"] = "ویرایش با موفقیت انجام شد";
            return RedirectToAction("appointment", "Account");
        }

        public IActionResult DeleteAppointment(int id)
        {
            var data = _appointmentService.GetAppointmentById(id);
            return View(data);
        }

        [HttpPost]
        public IActionResult DeleteAppointment(Appointment appointment)
        {
            _appointmentService.DeleteAppointment(appointment);
            TempData["success"] = "حذف با موفقیت انجام شد";
            return RedirectToAction("appointment", "Account");
        }

        public IActionResult DetailAppointment(string sickId)
        {
            var data = new DetailAppointmentViewModel()
            {
                User = _userManager.FindByIdAsync(sickId).Result,
                Appointments = _appointmentService.GetAppointmentOfUser(sickId)
            };


            return View(data);
        }
    }
}