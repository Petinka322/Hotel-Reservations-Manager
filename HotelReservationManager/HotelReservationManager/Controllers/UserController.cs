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
    public class UserController:Controller
    {
        private readonly HotelDbContext _context;

        public UserController(HotelDbContext context)
        {
            _context = context;
        }
      
        //Get: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }
       
        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Password,Username,First_name,Second_name,Last_name,EGN,Phone,E_mail,Hire_date,Is_active,Release_date")] Users user)
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
        public async Task<IActionResult> Delete(string? EGN)
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
    }
}
