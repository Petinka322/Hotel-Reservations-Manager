using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservationManager.Models;

namespace HotelReservationManager.Repository
{
    public interface ILogin
    {
        Task<IEnumerable<Users>> getuser();
        Task<Users> AuthenticateUser(string username, string password);
    }
}
