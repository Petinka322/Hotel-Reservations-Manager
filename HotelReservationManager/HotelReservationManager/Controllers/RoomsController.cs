using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservationManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationManager.Controllers
{
    public class RoomsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly HotelDbContext _context;

        public ReservationController(HotelDbContext context)
        {
            _context = context;
        }

        // POST: Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomsCapacity,RoomsType,Is_Available,Price_Adult,Price_Child")] Rooms rooms)
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
        public async Task<IActionResult> Create()
        {
            return View();
        }
        // GET: Rooms/Delete
        public async Task<IActionResult> Delete(int? RoomId)
        {
            if (RoomId == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var rooms = await _context.Rooms
                .FirstOrDefaultAsync(m => m.RoomId == RoomId);
            if (rooms == null)
            {
                return NotFound();
            }

            return View(rooms);
        }
        // POST: Rooms/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? RoomId)
        {
            if (_context.Reservations == null)
            {
                return Problem("This reservation is missing.");
            }
            var rooms = await _context.Rooms.FindAsync(RoomId);
            if (rooms != null)
            {
                _context.Rooms.Remove(rooms);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Rooms/Edit
        public async Task<IActionResult> Edit(int? RoomId)
        {
            if (RoomId == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var rooms = await _context.Users.FindAsync(RoomId);
            if (rooms == null)
            {
                return NotFound();
            }
            return View(rooms);
        }
        // POST: Rooms/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int RoomId, [Bind("RoomsId,RoomsCapacity,RoomsType,Is_Available,Price_Adult,Price_Child")] Rooms rooms)
        {
            if (RoomId != Rooms.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rooms);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Exists(rooms.RoomsId))
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
            return View(rooms);
        }
        // GET: Rooms/Details/
        public async Task<IActionResult> DetailsReservation(int? RoomId)
        {
            if (RoomId == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var rooms = await _context.Rooms
                .FirstOrDefaultAsync(m => m.RoomId == RoomId);
            if (rooms == null)
            {
                return NotFound();
            }

            return View(rooms);
        }
        private bool Exists(int id)
        {
            return _context.Rooms.Any(e => e.RoomsId == id);
        }
    }
}
