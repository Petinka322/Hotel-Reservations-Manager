﻿using System;
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
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;

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
            ViewBag.UsernameSortParm = String.IsNullOrEmpty(sortOrder) ? "username_desc" : "";
            ViewBag.First_NameSortParm = String.IsNullOrEmpty(sortOrder) ? "first_name_desc" : "";
            ViewBag.Second_NameSortParm = String.IsNullOrEmpty(sortOrder) ? "second_name_desc" : "";
            ViewBag.Last_NameSortParm = String.IsNullOrEmpty(sortOrder) ? "last_name_desc" : "";
            ViewBag.E_mailSortParm = String.IsNullOrEmpty(sortOrder) ? "E_mail_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var users = from s in _context.Users select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Username.Contains(searchString)
                                       || s.First_name.Contains(searchString)
                                       || s.Second_name.Contains(searchString)
                                       || s.Last_name.Contains(searchString)
                                       || s.E_mail.Contains(searchString));
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
                case "Username":
                    users = users.OrderBy(s => s.Username);
                    break;
                case "Username_desc":
                    users = users.OrderByDescending(s => s.Username);
                    break;
                case "E_mail":
                    users = users.OrderBy(s => s.E_mail);
                    break;
                case "E_mail_desc":
                    users = users.OrderByDescending(s => s.E_mail);
                    break;
                default:
                    users = users.OrderBy(s => s.First_name);
                    break;
            }

            int pageSize = 10;
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
            Regex regex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            if (user.EGN.Length != 10)
            {
                return Problem("EGN cannot be shorter than 10 digits!");
            }
            if (user.Phone.Length !=10)
            {
                return Problem("Phone number cannot be longer or shorter than 10 digits!");
            }
            if (regex.IsMatch(user.E_mail) == false)
            {
                return Problem("This is not an email!");
            }
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
        public async Task<IActionResult> Delete(string? id)
        {
            Console.WriteLine("Inside Delete action method.");
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.EGN == id);
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
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Users == null)
            {
                return Problem("This user is missing.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: User/Details/
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.EGN == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        // POST: User/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Username,Password,First_name,Second_name,Last_name,Is_Administrator,EGN,Phone,E_mail,Hire_date,Is_active,Release_date")] Users user)
        {
            Regex regex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

            if (user.EGN.Length < 10)
            {
                return Problem("EGN cannot be shorter than 10 digits!");
            }
            if (user.Phone.Length != 10)
            {
                return Problem("Phone number cannot be longer or shorter than 10 digits!");
            }
            if (regex.IsMatch(user.E_mail) == false)
            {
                return Problem("This is not an email!");
            }
            if (user.Hire_date >= user.Release_date)
            {
                return Problem("The date of hire cannot be the same or later than the date of release!");
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
        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.EGN == id);
        }
    }
}
