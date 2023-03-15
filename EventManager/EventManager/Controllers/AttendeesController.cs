using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventManager.Data;

namespace EventManager.Controllers
{
    public class AttendeesController : Controller
    {
        private readonly EventDbContext _context;

        public AttendeesController(EventDbContext context)
        {
            _context = context;
        }

        // GET: Attendees
        public async Task<IActionResult> Index()
        {
              return View(await _context.Attendees.ToListAsync());
        }

        // GET: Attendees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Attendees == null)
            {
                return NotFound();
            }

            var attendee = await _context.Attendees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendee == null)
            {
                return NotFound();
            }

            return View(attendee);
        }

        // GET: Attendees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Attendees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email")] Attendee attendee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(attendee);
        }

        // GET: Attendees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Attendees == null)
            {
                return NotFound();
            }

            var attendee = await _context.Attendees.FindAsync(id);
            if (attendee == null)
            {
                return NotFound();
            }
            return View(attendee);
        }

        // POST: Attendees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email")] Attendee attendee)
        {
            if (id != attendee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendeeExists(attendee.Id))
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
            return View(attendee);
        }

        // GET: Attendees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Attendees == null)
            {
                return NotFound();
            }

            var attendee = await _context.Attendees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendee == null)
            {
                return NotFound();
            }

            return View(attendee);
        }

        // POST: Attendees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Attendees == null)
            {
                return Problem("Entity set 'EventDbContext.Attendees'  is null.");
            }
            var attendee = await _context.Attendees.FindAsync(id);
            if (attendee != null)
            {
                _context.Attendees.Remove(attendee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Attendees/Associate
        public async Task<IActionResult> Associate()
        {
            var events = await _context.Events.ToListAsync();
            var attendees = await _context.Attendees.ToListAsync();

            ViewBag.Events = new SelectList(events, "Id", "Name");
            ViewBag.Attendees = new SelectList(attendees, "Id", "Name");

            return View();
        }

        // POST: Attendees/Associate
        [HttpPost]
        public async Task<IActionResult> Associate(int eventId, int attendeeId)
        {
            var attendee = await _context.Attendees.FindAsync(attendeeId);
            var @event = await _context.Events.FindAsync(eventId);

            if (attendee == null || @event == null)
            {
                return NotFound();
            }

            if (!@event.Attendees.Contains(attendee))
            {
                @event.Attendees.Add(attendee);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AttendeeExists(int id)
        {
          return _context.Attendees.Any(e => e.Id == id);
        }
    }
}
