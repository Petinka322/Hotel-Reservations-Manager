
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
    public class ManagerControllers : Controller
    {
        private readonly HotelDbContext _context;

        public ManagerControllers(HotelDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        // GET: Users/Create
        public async Task<IActionResult> CreateUser()
        {
            return View(await _context.Users.ToListAsync());
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser([Bind("Password,Username,First_name,Second_name,Last_name,EGN,Phone,E_mail,Hire_date,Is_active,Release_date")] Users user)
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
        public async Task<IActionResult> DeleteUSER(string? EGN)
        {
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
        private bool UserExists(string EGN)
        {
            return _context.Users.Any(e => e.EGN == EGN);
        }
        // POST: Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoom([Bind("RoomsId, RoomsCapacity, RoomsType, Is_Available, Price_Adult, Price_Child")] Rooms rooms)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rooms);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rooms);
        }
        // GET: Rooms/Create
        public async Task<IActionResult> CreateRooms()
        {
            return View(await _context.Rooms.ToListAsync());
        }

        // POST: Reservation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReservation([Bind("Username, Arrval_Date, Departure_Date, Breakfast, All_Inclusive, Price")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }
        // GET: Reservation/Create
        public async Task<IActionResult> CreateReservation()
        {
            return View(await _context.Reservations.ToListAsync());
        }
        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClients([Bind("ClientId, First_Name, Last_Name, Phone, E_mail, Adult")] Clients clients)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clients);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clients);
        }
        // GET: Clients/Create
        public async Task<IActionResult> CreateClients()
        {
            return View(await _context.Clients.ToListAsync());
        }
    }
}
