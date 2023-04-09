using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HotelReservationManager.Models;
using HotelReservationManager.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HotelReservationManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogin _loginUser;
        private readonly HotelDbContext _context;

        public HomeController(HotelDbContext context, ILogin logger)
        {
            _context = context;
            _loginUser = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string password)
        {

            var status = _context.Users.Where(m => m.Username == username && m.Password == password).FirstOrDefault();
            LogInfo info = new LogInfo();
            info.Set(status.Is_Administrator);
            if (status != null)
            {
                if (status.Is_Administrator == true && status.Is_active == true)
                {
                    ViewBag.Message = "Successfull login";
                    return View("Manager");
                }
                else if(status.Is_active == true)
                {
                    ViewBag.Message = "Successfull login";
                    return View("User");
                }
            }
            else
            {
                ViewBag.Message = "Invalid login detail.";
            }
            return View();
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
    }
}