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
    public class UserControllers:Controller
    {
        private readonly HotelDbContext _context;

        public UserControllers(HotelDbContext context)
        {
            _context = context;
        }
        public IActionResult Create()
        {
            return View();
        }
        // GET: Users
        public async Task<IActionResult> ClientsIndex()
        {
            return View(await _context.Clients.ToListAsync());
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClient([Bind("ClientId,First_name,Last_name,Phone,E_mail,Adult")] Clients client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }
        // GET: Client/Delete/5
        public async Task<IActionResult> DeleteClient(int? ClientId)
        {
            if (ClientId == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.ClientId == ClientId);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClientDeleteConfirmed(int ClientId)
        {
            if (_context.Clients == null)
            {
                return Problem("This client is missing.");
            }
            var client = await _context.Clients.FindAsync(ClientId);
            if (client != null)
            {
                _context.Clients.Remove(client);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Reserve()
        {
            var reservations = await _context.Reservations.ToListAsync();
            var clients = await _context.Clients.ToListAsync();

            ViewBag.Reservations = new SelectList(reservations, "ResId");
            ViewBag.Clients = new SelectList(clients, "ClientId", "First_name");

            return View();
        }

        // POST: Clients/Reserve
        [HttpPost]
        public async Task<IActionResult> Reserve(int ResId, int ClientId)
        {
            var Client = await _context.Clients.FindAsync(ClientId);
            var @reservation = await _context.Reservations.FindAsync(ResId);

            if (Client == null || @reservation == null)
            {
                return NotFound();
            }

            if (!@reservation.Clients.Contains(Client))
            {
                @reservation.Clients.Add(Client);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int ClientId)
        {
            return _context.Clients.Any(e => e.ClientId == ClientId);
        }
    }
}
