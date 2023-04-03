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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clients.ToListAsync());
        }
        //public IActionResult Create()
        //{
        //    return View();
        //}
        //// GET: Users


        //// POST: Users/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateClient([Bind("ClientId,First_name,Last_name,Phone,E_mail,Adult")] Clients client)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(client);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(client);
        //}
        //// GET: Client/Delete
        //public async Task<IActionResult> DeleteClient(int? ClientId)
        //{
        //    if (ClientId == null || _context.Clients == null)
        //    {
        //        return NotFound();
        //    }

        //    var client = await _context.Clients
        //        .FirstOrDefaultAsync(m => m.ClientId == ClientId);
        //    if (client == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(client);
        //}

        //// POST: Clients/Delete
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ClientDeleteConfirmed(int ClientId)
        //{
        //    if (_context.Clients == null)
        //    {
        //        return Problem("This client is missing.");
        //    }
        //    var client = await _context.Clients.FindAsync(ClientId);
        //    if (client != null)
        //    {
        //        _context.Clients.Remove(client);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
        //public async Task<IActionResult> Reserve()
        //{
        //    var reservations = await _context.Reservations.ToListAsync();
        //    var clients = await _context.Clients.ToListAsync();

        //    ViewBag.Reservations = new SelectList(reservations, "ResId");
        //    ViewBag.Clients = new SelectList(clients, "ClientId", "First_name");

        //    return View();
        //}

        //// POST: Clients/Reserve
        //[HttpPost]
        //public async Task<IActionResult> Reserve(int ResId, int ClientId)
        //{
        //    var Client = await _context.Clients.FindAsync(ClientId);
        //    var @reservation = await _context.Reservations.FindAsync(ResId);

        //    if (Client == null || @reservation == null)
        //    {
        //        return NotFound();
        //    }

        //    if (!@reservation.Clients.Contains(Client))
        //    {
        //        @reservation.Clients.Add(Client);
        //    }

        //    return RedirectToAction(nameof(Index));
        //}

        //private bool UserExists(int ClientId)
        //{
        //    return _context.Clients.Any(e => e.ClientId == ClientId);
        //}
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
            return View();
        }
        // GET: Reservation/Delete
        public async Task<IActionResult> DeleteReservation(int? resId)
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
        public async Task<IActionResult> DeleteResConfirmed(int? resId)
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
        public async Task<IActionResult> EditReservation(int? resId)
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
        public async Task<IActionResult> EditReservation(int resId, [Bind("ResId,RoomsId,Username,Clients,Arrival_date, Departure_date,Breakfast,All_inclusive,Price")] Reservation reservation)
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
                    if (!ReservationExists(reservation.ResId))
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
            return View();
        }
        // GET: Clients/Delete
        public async Task<IActionResult> DeleteClient(int? clientId)
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
        public async Task<IActionResult> DeleteClientConfirmed(int? clientId)
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
        public async Task<IActionResult> EditClients(int? clientID)
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
        public async Task<IActionResult> EditClients(int clientId, [Bind("ClientId,First_Name,Last_Name,Phone,E_mail, Adult,Reservations")] Clients client)
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
        // GET: Reservation/Details/
        public async Task<IActionResult> DetailsClient(int? clientId)
        {
            if (clientId == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.ClientId == clientId);
            if (clientId == null)
            {
                return NotFound();
            }

            return View(client);
        }
        // GET: Attendees/Associate
        public async Task<IActionResult> Associate()
        {
            var events = await _context.Reservations.ToListAsync();
            var attendees = await _context.Clients.ToListAsync();

            ViewBag.Events = new SelectList(events, "Id", "Name");
            ViewBag.Attendees = new SelectList(attendees, "Id", "Name");

            return View();
        }

        // POST: Clients/Associate
        /*
        [HttpPost]
        public async Task<IActionResult> Associate(int resId, int clientId)
        {
            var client = await _context.Clients.FindAsync(clientId);
            var reservation = await _context.Reservations.FindAsync(resId);

            if (client == null || reservation == null)
            {
                return NotFound();
            }

            var contains = _context.ReservationClients
                .Any(ea => ea.ReservationId == reservation.ResId && ea.AttendeeId == reservation.Clients);

            if (!contains)
            {
                @reservation.ReservationClient.Add(new ReservationClient()
                {
                    ClientId = Client.Id,
                    EventId = @event.Id
                });
            }
            return RedirectToAction(nameof(Index));
        }
        */
        private bool ReservationExists(int resId)
        {
            return _context.Reservations.Any(e => e.ResId == resId);
        }
        private bool ClientExists(int ClientId)
        {
            return _context.Clients.Any(e => e.ClientId == ClientId);
        }
        
    }
}
