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
    public class ManagerController : Controller
    {
        private readonly HotelDbContext _context;

        public ManagerController(HotelDbContext context)
        {
            _context = context;
        }
        //Get: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }
        // GET: Users/Create
        public async Task<IActionResult> CreateUser()
        {
            return View();
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
        public async Task<IActionResult> DeleteUser(string? EGN)
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
        // GET: User/Details/
        public async Task<IActionResult> Details(string? egn)
        {
            if (egn == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.EGN == egn);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        // GET: Users/Edit
        public async Task<IActionResult> Edit(string? egn)
        {
            if (egn == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(egn);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        // POST: User/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string egn, [Bind("Password,Username,First_name,Second_name,Last_name, EGN,Phone, E_mail,Hire_date,Is_active, Release_date")] Users user)
        {
            if (egn != user.EGN)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.EGN))
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
            return View(user);
        }
        private bool UserExists(string EGN)
        {
            return _context.Users.Any(e => e.EGN == EGN);
        }
        private bool RoomExists(int roomId)
        {
            return _context.Rooms.Any(e => e.RoomsId == roomId);
        }
        private bool ReservationExists(int resId)
        {
            return _context.Reservations.Any(e => e.ResId == resId);
        }
        private bool ClientExists(int clientId)
        {
            return _context.Clients.Any(e => e.ClientId == clientId);
        }
        // GET: Rooms/Create
        public async Task<IActionResult> CreateRooms()
        {
            return View();
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

        // GET: Room/Delete
        public async Task<IActionResult> DeleteRoom(int? roomId)
        {
            if (roomId == null || _context.Users == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.RoomsId == roomId);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }
        // POST: Room/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.Rooms == null)
            {
                return Problem("This user is missing.");
            }
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Rooms/Details/
        public async Task<IActionResult> Details(int? roomId)
        {
            if (roomId == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.RoomsId == roomId);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }
        // GET: Rooms/Edit
        public async Task<IActionResult> Edit(int? roomId)
        {
            if (roomId == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Users.FindAsync(roomId);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }
        // POST: Rooms/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int roomId, [Bind("RoomsId,RoomsCapacity,RoomsType,Is_Available,Price_Adult, Price_Child")] Rooms room)
        {
            if (roomId != room.RoomsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.RoomsId))
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
            return View(room);
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
    }
}
