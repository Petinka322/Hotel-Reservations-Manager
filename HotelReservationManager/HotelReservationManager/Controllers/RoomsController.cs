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
    public class RoomsController : Controller
    {
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.SortOrder = sortOrder;
            ViewBag.RoomsTypeSortParm = String.IsNullOrEmpty(sortOrder) ? "RoomsType_desc" : "";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var rooms = from s in _context.Rooms
                               select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                rooms = rooms.Where(s => s.RoomsType.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "RoomsType_desc":
                    rooms = rooms.OrderByDescending(s => s.RoomsType);
                    break;
                default:
                    rooms = rooms.OrderBy(s => s.RoomsType);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(rooms.ToPagedList(pageNumber, pageSize));
        }

        private readonly HotelDbContext _context;

        public RoomsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomsId,RoomsCapacity,RoomsType,Is_Available,Price_Adult,Price_Child")] Rooms rooms)
        {
            if (rooms.RoomsCapacity <= 0) 
            {
                return Problem("The room cannot have 0 or less than 0 capacity!");
            }
            if (rooms.Price_Adult <= rooms.Price_Child) 
            {
                return Problem("The Price for an Adult cannot be larger than the Price for a Child!");
            }
            if (ModelState.IsValid)
            {
                _context.Add(rooms);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rooms);
        }
        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? RoomId)
        {
            if (RoomId == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var rooms = await _context.Rooms
                .FirstOrDefaultAsync(m => m.RoomsId == RoomId);
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
            if (RoomId != rooms.RoomsId)
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
        public async Task<IActionResult> Details(int? RoomId)
        {
            if (RoomId == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var rooms = await _context.Rooms
                .FirstOrDefaultAsync(m => m.RoomsId == RoomId);
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
