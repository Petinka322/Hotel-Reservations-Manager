using HotelReservationManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotelReservationManager.Repository;

namespace HotelReservationManager.Repository
{
    public class AuthenticateLogin : ILogin
    {
        private readonly HotelDbContext _context;

        public AuthenticateLogin(HotelDbContext context)
        {
            _context = context;
        }
        public async Task<Users> AuthenticateUser(string username, string password)
        {
            var succeeded = await _context.Users.FirstOrDefaultAsync(authUser => authUser.Username == username && authUser.Password == password);
            return succeeded;
        }

        public async Task<IEnumerable<Users>> getuser()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
