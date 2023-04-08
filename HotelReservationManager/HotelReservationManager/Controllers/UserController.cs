using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelReservationManager.Models;
using X.PagedList;
using HotelReservationManager.Migrations;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace HotelReservationManager.Controllers
{
    public class UserController:Controller
    {
        private readonly HotelDbContext _context;
        public UserController(HotelDbContext context)
        {
            _context = context;
        }

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.SortOrder = sortOrder;
            ViewBag.First_NameSortParm = String.IsNullOrEmpty(sortOrder) ? "first_name_desc" : "";
            ViewBag.Second_NameSortParm = String.IsNullOrEmpty(sortOrder) ? "second_name_desc" : "";
            ViewBag.Last_NameSortParm = String.IsNullOrEmpty(sortOrder) ? "last_name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var users = from s in _context.Users
                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.First_name.Contains(searchString)
                                       || s.Second_name.Contains(searchString)
                                       || s.Last_name.Contains(searchString));  
            }
            switch (sortOrder)
            {
                case "first_name_desc":
                    users = users.OrderByDescending(s => s.First_name);
                    break;
                case "second_name":
                    users = users.OrderBy(s => s.Second_name);
                    break;
                case "second_name_desc":
                    users = users.OrderByDescending(s => s.Second_name);
                    break;
                case "last_name":
                    users = users.OrderBy(s => s.Last_name);
                    break;
                case "last_name_desc":
                    users = users.OrderByDescending(s => s.Last_name);
                    break;
                default:
                    users = users.OrderBy(s => s.First_name);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(users.ToPagedList(pageNumber, pageSize));
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Password,Username,First_name,Second_name,Last_name,Is_Administrator,EGN,Phone,E_mail,Hire_date,Is_active,Release_date")] Users user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        // GET: Users/Delete
        [HttpGet]
        public async Task<IActionResult> Delete(string? EGN)
        {
            Console.WriteLine("Inside Delete action method.");
            if (EGN == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.EGN == EGN);
            if (user == null)
            {
                return NotFound();
            }
            Console.WriteLine("Inside Delete action method.");
            return View(user);
        }
        // POST: Users/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string EGN)
        {
            if (_context.Users == null)
            {
                return Problem("This user is missing.");
            }
            var user = await _context.Users.FindAsync(EGN);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: User/Details/
        public async Task<IActionResult> Details(string? egn)
        {
            if (egn == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.EGN == egn);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string? egn)
        {
            if (egn == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(egn);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        // POST: User/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string egn, [Bind("Password,Username,First_name,Second_name,Last_name, EGN,Phone, E_mail,Hire_date,Is_active, Release_date")] Users user)
        {
            if (egn != user.EGN)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.EGN))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        private bool UserExists(string EGN)
        {
            return _context.Users.Any(e => e.EGN == EGN);
        }
    }
}
