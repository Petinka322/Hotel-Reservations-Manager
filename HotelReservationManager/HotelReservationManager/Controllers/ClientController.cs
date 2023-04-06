using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelReservationManager.Models;

namespace HotelReservationManager.Controllers
{
    public class ClientController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clients.ToListAsync());
        }

        private readonly HotelDbContext _context;

        public ClientController(HotelDbContext context)
        {
            _context = context;
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("First_Name, Last_Name, Phone, E_mail, Adult")] Clients clients)
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
        public IActionResult Create()
        {
            return View();
        }
        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? clientId)
        {
            if (clientId == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }
        // POST: Client/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? clientId)
        {
            if (_context.Clients == null)
            {
                return Problem("This client is missing.");
            }
            var clinet = await _context.Clients.FindAsync(clientId);
            if (clinet != null)
            {
                _context.Clients.Remove(clinet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Client/Edit
        public async Task<IActionResult> Edit(int? clientID)
        {
            if (clientID == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(clientID);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }
        // POST: Client/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int clientId, [Bind("ClientId,First_Name,Last_Name,Phone,E_mail, Adult,Reservations")] Clients client)
        {
            if (clientId != client.ClientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.ClientId))
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
            return View(client);
        }
        private bool ReservationExists(int resId)
        {
            return _context.Reservations.Any(e => e.ResId == resId);
        }
        private bool ClientExists(int ClientId)
        {
            return _context.Clients.Any(e => e.ClientId == ClientId);
        }

        // GET: Clients/Associate
        public async Task<IActionResult> Associate()
        {
            var reservations = await _context.Reservations.ToListAsync();
            var clients = await _context.Clients.ToListAsync();

            ViewBag.Events = new SelectList(reservations, "Id", "Name");
            ViewBag.Attendees = new SelectList(clients, "Id", "Name");

            return View();
        }

        // POST: Clients/Associate
        [HttpPost]
        public async Task<IActionResult> Associate(int eventId, int attendeeId)
        {
            var client = await _context.Clients.FindAsync(attendeeId);
            var reservation = await _context.Reservations.FindAsync(eventId);

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
