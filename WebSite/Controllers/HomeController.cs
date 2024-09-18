using Application.Dtos;
using Application.Interfaces.Appointments;
using Application.Interfaces.Blogs;
using Application.Interfaces.Categories;
using Application.Interfaces.Contexts;
using Application.Interfaces.Payments;
using Application.Interfaces.Users;
using Domain.Appointments;
using Domain.Payments;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using WebSite.Models;
using WebSite.Models.ViewModels.Home;
using WebSite.Models.ViewModels.User;

namespace WebSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IDataBaseContext _dataBaseContext;
        private readonly IAppointmentService _appointmentService;
        private readonly IPaymentService _paymentService;


        public HomeController(ILogger<HomeController> logger, IBlogService blogService, ICategoryService categoryService,
            UserManager<User> userManager, IUserService userService, IDataBaseContext dataBaseContext,
            IAppointmentService appointmentService, IPaymentService paymentService)
        {
            _logger = logger;
            _blogService = blogService;
            _categoryService = categoryService;
            _userManager = userManager;
            _userService = userService;
            _dataBaseContext = dataBaseContext;
            _appointmentService = appointmentService;
            _paymentService = paymentService;

        }


        public IActionResult Index()
        {
            var data = new Models.ViewModels.Index.IndexViewModel()
            {
                Categories = _categoryService.GetAllCategory(),
                Users = _userManager.Users.Where(u => u.IsDoctor && !u.IsDelete).ToList(),
                Blogs = _blogService.GetAllBlogsForWebSite(),
                Comments = _userService.GetAllCommentForWebSite().Take(5).ToList(),
            };
            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ListOfDoctors(DoctorRequestDto doctor)
        {
            var query = _userManager.Users.Where(u => u.IsDoctor && !u.IsDelete).AsQueryable();

            if (!string.IsNullOrEmpty(doctor.filterFullName))
            {
                query = query.Where(u => u.FullName.Contains(doctor.filterFullName));
            }

            if (!string.IsNullOrEmpty(doctor.filterExpertise))
            {
                query = query.Where(u => u.Expertise.Contains(doctor.filterExpertise));
            }

            var data = new ListOfDoctorsViewModel()
            {
                Users = query.ToList(),
            };

            return View(data);
        }

        public IActionResult Reservation(string userId)
        {
            var data = new Models.ViewModels.Home.ReservationViewModel()
            {
                User = _userManager.FindByIdAsync(userId).Result,
                TextualAppointments = _appointmentService.GetTextualAppointment(userId),
                ByPhoneAppointments = _appointmentService.GetByPhoneAppointment(userId),
            };
            return View(data);
        }

        public IActionResult Blogs(int? pageNumber)
        {
            var data = _blogService.GetAllBlogsForWebSite(pageNumber);
            return View(data);
        }
        public IActionResult BlogDetail(int id)
        {
            var data = _blogService.GetBlogDetail(id);
            return View(data);
        }

        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateComment(Comment newComment)
        {
            newComment.UserId = _userManager.GetUserId(User);
            newComment.InsertDate = DateTime.Now;
            newComment.IsDelete = false;
            newComment.IsAdminRead = false;

            _dataBaseContext.Comments.Add(newComment);
            _dataBaseContext.SaveChanges();

            //_userService.AddComment(newComment);

            return View("ShowComment", _userService.GetComment(newComment.DoctorId,1));
        }

        public IActionResult ShowComment(string id,int? pageNumber)
        {
            var comments = _userService.GetComment(id, pageNumber);
            return View(comments);
        }


        [Authorize]
        [HttpPost]
        public IActionResult PaymentGetWay(int appointmentId, int amount)
        {
            Payment newPayment = new Payment();
            newPayment.AppointmentId = appointmentId;
            newPayment.Amount = amount;

            _paymentService.AddPayment(newPayment);

            var payment = new ZarinpalSandbox.Payment(amount);

            var result = payment.PaymentRequest("نوبت", Url.Action(nameof(OnlinePayment), "Home", new { newPayment.Id , appointmentId }, protocol: Request.Scheme));

            //var result = payment.PaymentRequest("Test" , "https://localhost:5268/Home/OnlinePayment" + newPayment.Id);

            if (result.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + result.Result.Authority);
            }

            //var payment = _paymentService.PayForAppointment(appointmentId);

            //return RedirectToAction("Index" , "Pay" , new {PaymentId = payment.PaymentId});
            TempData["error"] = "عملیات با خطا مواجه شد";
            return View();
        }

        [Authorize]
        public IActionResult OnlinePayment(Guid id , int appointmentId)
        {
           

            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok"
                && HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"];

                var payment = _paymentService.GetPaymentById(id);
                var appointment = _appointmentService.GetAppointmentById(appointmentId);

                var pay = new ZarinpalSandbox.Payment(payment.Amount);
                var res = pay.Verification(authority).Result;
                if (res.Status == 100)
                {
                    payment.RefId = res.RefId;
                    ViewBag.IsSuccess = true;
                    payment.IsPay = true;
                    payment.Authority = authority;
                    payment.DatePay = DateTime.Now;
                    _paymentService.UpdatePayment(payment);    
                    appointment.IsReserved = true;
                    appointment.SickId=_userManager.GetUserId(User);
                    _appointmentService.UpdateAppointment(appointment);
                    TempData["success"] = " پرداخت موفقیت انجام شد";
                }
           

            }
 
            var getAppointment = _appointmentService.GetAppointmentById(appointmentId);

            if (!getAppointment.IsReserved)
            {
                TempData["error"] = "عملیات با شکست مواجه شد.";
            }
          

            return RedirectToAction("Reservation" , "Home" , new { getAppointment.UserId});
        }
    }
}
