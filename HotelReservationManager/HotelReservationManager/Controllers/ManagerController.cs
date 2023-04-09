using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservationManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationManager.Controllers
{
    public class ManagerController : Controller
    {
        private readonly HotelDbContext _context;
        public ManagerController(HotelDbContext context)
        {
            _context = context;
        }
        

        public IActionResult Index()
        {
            LogInfo info = new LogInfo();
            Console.WriteLine(info);
            if (info.Get())
            {
                return View("Manager");
            }
            else 
            {
                return View("User");
            }
            
        }
        public IActionResult Manager()
        {
            return View("Manager");
        }
        public IActionResult User()
        {
            return View("User");
        }
    }
}
