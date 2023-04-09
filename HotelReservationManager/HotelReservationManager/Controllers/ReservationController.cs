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
using Microsoft.Extensions.Logging;

namespace HotelReservationManager.Controllers
{
    public class ReservationController : Controller
    {
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.SortOrder = sortOrder;
            ViewBag.UsernameSortParm = String.IsNullOrEmpty(sortOrder) ? "Username_desc" : "";
            ViewBag.RoomsIdSortParm = String.IsNullOrEmpty(sortOrder) ? "RoomsId_desc" : "";
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
                reservations = reservations.Where(s => s.Username.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Username_desc":
                    reservations = reservations.OrderByDescending(s => s.Username);
                    break;
                case "RoomsId_desc":
                    reservations = reservations.OrderByDescending(s => s.RoomsId);
                    break;
                case "Username":
                    reservations = reservations.OrderBy(s => s.Username);
                    break;
                default:
                    reservations = reservations.OrderBy(s => s.RoomsId);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(reservations.ToPagedList(pageNumber, pageSize));
        }

        private readonly HotelDbContext _context;

        public ReservationController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: Reservation/Create
        public async Task<IActionResult> Create()
        {
            var room = await _context.Rooms.ToListAsync();
            ViewBag.Rooms = new SelectList(room, "RoomsId", "RoomsId");
            var user = await _context.Users.ToListAsync();
            ViewBag.Users = new SelectList(user, "Username", "Username");

            return View();
        }
        // POST: Reservation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ResId,RoomsId,Username,Arrival_date,Departure_date, Breakfast, All_Inclusive, Price")] Reservation reservation)
        {
            var room = _context.Rooms.Where(m => m.RoomsId == reservation.RoomsId).FirstOrDefault();
            if (reservation.RoomsId <= 0)
            {
                return Problem("The room number cannot be below or equal to 0!");
            }
            if (reservation.RoomsId != room.RoomsId)
            {
                return Problem("The room number doesn't exist!");
            }
            if (reservation.Departure_date < reservation.Arrival_date)
            {
                return Problem("The reservation arrival date cannot be later than the departure date!");
            }
            if (ModelState.IsValid)
            {
                
                if (reservation.All_inclusive == true && reservation.Clients != null)
                {
                    reservation.Price += reservation.Departure_date.Subtract(reservation.Arrival_date).Days * 25;
                }
                if (reservation.Breakfast == true && reservation.Clients != null)
                {
                    reservation.Price += reservation.Departure_date.Subtract(reservation.Arrival_date).Days * 15;
                }
                reservation.Price += ((room.Price_Adult * reservation.Clients.Count(m => m.Adult) + (room.Price_Child * reservation.Clients.Count(m => !m.Adult))) * (reservation.Departure_date.Subtract(reservation.Arrival_date).Days));
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }
        // GET: Reservation/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.ResId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }
        // POST: Reservation/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.Reservations == null)
            {
                return Problem("This reservation is missing.");
            }
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Reservation/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            var room = await _context.Reservations.ToListAsync();
            ViewBag.Rooms = new SelectList(room, "RoomsId", "RoomsId");
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }
        // POST: Reservation/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ResId,RoomsId,Username,Clients,Arrival_date,Departure_date,Breakfast,All_inclusive,Price")] Reservation reservation)
        {
            var room = _context.Rooms.Where(m => m.RoomsId == reservation.RoomsId).FirstOrDefault();
            if (reservation.RoomsId <= 0)
            {
                return Problem("The room number cannot be below or equal to 0!");
            }
            if (reservation.RoomsId != room.RoomsId )
            {
                return Problem("The room number doesn't exist!");
            }
            if (reservation.Arrival_date >= reservation.Departure_date)
            {
                return Problem("The reservation arrival date cannot be the same or earlier than the departure date!");
            }
            if (ModelState.IsValid)
            {
               
                if (reservation.All_inclusive == true && reservation.Clients != null )
                {
                    reservation.Price += reservation.Departure_date.Subtract(reservation.Arrival_date).Days * 25;
                }
                if (reservation.Breakfast == true && reservation.Clients != null)
                {
                    reservation.Price += reservation.Departure_date.Subtract(reservation.Arrival_date).Days * 15;
                }
                reservation.Price += ((room.Price_Adult * reservation.Clients.Count(m => m.Adult) + (room.Price_Child * reservation.Clients.Count(m => !m.Adult))) * (reservation.Departure_date.Subtract(reservation.Arrival_date).Days));
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
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.ResId == id);
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
        // GET: Clients/Associate
        public async Task<IActionResult> Associate()
        {
            var reservations = await _context.Reservations.ToListAsync();
            var clients = await _context.Clients.ToListAsync();

            ViewBag.Reservations = new SelectList(reservations, "ResId", "RoomsId");
            ViewBag.Clients = new SelectList(clients, "ClientId", "First_Name");

            return View();
        }

        // POST: Clients/Associate
        [HttpPost]
        public async Task<IActionResult> Associate(int resId, int clientId)
        {
            var client = await _context.Clients.FindAsync(clientId);
            var reservation = await _context.Reservations.FindAsync(resId);

            if (client == null || reservation == null)
            {
                return NotFound();
            }

            if (!reservation.Clients.Contains(client))
            {
                reservation.Clients.Add(client);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
