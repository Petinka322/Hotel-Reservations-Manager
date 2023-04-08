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
    public class ClientController : Controller
    {
        public ViewResult Index(string sortOrder,string currentFilter, string searchString, int? page)
        {
            ViewBag.SortOrder = sortOrder;
            ViewBag.First_NameSortParm = String.IsNullOrEmpty(sortOrder) ? "first_name_desc" : "";
            ViewBag.Last_NameSortParm = String.IsNullOrEmpty(sortOrder) ? "last_name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var clients = from s in _context.Clients
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(s => s.First_Name.Contains(searchString)
                                       || s.Last_Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "first_name_desc":
                    clients = clients.OrderByDescending(s => s.First_Name);
                    break;
                case "last_name":
                    clients = clients.OrderBy(s => s.Last_Name);
                    break;
                case "last_name_desc":
                    clients = clients.OrderByDescending(s => s.Last_Name);
                    break;
                default:
                    clients = clients.OrderBy(s => s.First_Name);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(clients.ToPagedList(pageNumber, pageSize));
        }

        private readonly HotelDbContext _context;

        public ClientController(HotelDbContext context)
        {
            _context = context;
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientId, First_Name, Last_Name, Phone, E_mail, Adult")] Clients clients)
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

            var Clients = await _context.Clients
                .FirstOrDefaultAsync(m => m.ClientId == clientId);
            if (Clients == null)
            {
                return NotFound();
            }

            return View(Clients);
        }
        // POST: Client/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int clientId)
        {
            if (_context.Clients == null)
            {
                return Problem("This client is missing.");
            }
            var clients = await _context.Clients.FindAsync(clientId);
            if (clients != null)
            {
                _context.Clients.Remove(clients);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Client/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? clientID)
        {
            if (clientID == null || _context.Clients == null)
            {
                return NotFound();
            }

            var clients = await _context.Clients.FindAsync(clientID);
            if (clients == null)
            {
                return NotFound();
            }
            return View(clients);
        }
        // POST: Client/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int clientId, [Bind("ClientId, First_Name, Last_Name, Phone, E_mail, Adult,Reservations")] Clients clients)
        {
            if (clientId != clients.ClientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clients);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(clients.ClientId))
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
            return View(clients);
        }
        private bool ReservationExists(int resId)
        {
            return _context.Reservations.Any(e => e.ResId == resId);
        }
        private bool ClientExists(int ClientId)
        {
            return _context.Clients.Any(e => e.ClientId == ClientId);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? clientId)
        {
            if (clientId == null || _context.Clients == null)
            {
                return NotFound();
            }

            var clients = await _context.Clients
                .FirstOrDefaultAsync(m => m.ClientId == clientId);
            if (clients == null)
            {
                return NotFound();
            }

            return View(clients);
        }

        // GET: Clients/Associate
        public async Task<IActionResult> Associate()
        {
            var reservations = await _context.Reservations.ToListAsync();
            var clients = await _context.Clients.ToListAsync();

            ViewBag.Reservations = new SelectList(reservations, "ResId", "Usename");
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
