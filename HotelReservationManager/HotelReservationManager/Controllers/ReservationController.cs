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
    public class ReservationController : Controller
    {
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.SortOrder = sortOrder;
            ViewBag.UsenameSortParm = String.IsNullOrEmpty(sortOrder) ? "Usename_desc" : "";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var reservations = from s in _context.Reservations
                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                reservations = reservations.Where(s => s.Usename.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Usename_desc":
                    reservations = reservations.OrderByDescending(s => s.Usename);
                    break;
                default:
                    reservations = reservations.OrderBy(s => s.Usename);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(reservations.ToPagedList(pageNumber, pageSize));
        }

        private readonly HotelDbContext _context;

        public ReservationController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Reservation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username, Arrval_Date, Departure_Date, Breakfast, All_Inclusive, Price")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }
        // GET: Reservation/Delete
        public async Task<IActionResult> Delete(int? resId)
        {
            if (resId == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.ResId == resId);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }
        // POST: Reservation/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? resId)
        {
            if (_context.Reservations == null)
            {
                return Problem("This reservation is missing.");
            }
            var reservation = await _context.Rooms.FindAsync(resId);
            if (reservation != null)
            {
                _context.Rooms.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Reservation/Edit
        public async Task<IActionResult> Edit(int? resId)
        {
            if (resId == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Users.FindAsync(resId);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }
        // POST: Reservation/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int resId, [Bind("ResId,RoomsId,Username,Clients,Arrival_date, Departure_date,Breakfast,All_inclusive,Price")] Reservation reservation)
        {
            if (resId != reservation.ResId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Exists(reservation.ResId))
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
            return View(reservation);
        }
        // GET: Reservation/Details/
        public async Task<IActionResult> Details(int? resId)
        {
            if (resId == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.ResId == resId);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }
        private bool Exists(int id)
        {
            return _context.Reservations.Any(e => e.ResId == id);
        }
    }
}
