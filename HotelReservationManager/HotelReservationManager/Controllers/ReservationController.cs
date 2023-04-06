using HotelReservationManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationManager.Controllers
{
    public class ReservationController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reservations.ToListAsync());
        }

        private readonly HotelDbContext _context;

        public ReservationController(HotelDbContext context)
        {
            _context = context;
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
        public async Task<IActionResult> DetailsReservation(int? resId)
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
            return _context.Clients.Any(e => e.ClientId == id);
        }
    }
}
