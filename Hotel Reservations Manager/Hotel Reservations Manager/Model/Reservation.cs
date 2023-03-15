using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Hotel_Reservations_Manager.Model
{
    public class Reservation
    {

        public int ResId { get; set; }
        
        public int RoomId { get; set; }
       
        public string Usename { get; set; }
        public virtual HashSet<Clients> Clients { get; set; }
        public DateOnly Arrival_date    { get; set; }
        public DateOnly Departure_date { get; set; }
        public bool Breakfast { get; set; }
        public bool All_inclusive { get; set; }
        public double Price { get; set; }

        public Reservation()
        {
            Clients = new HashSet<Clients>();
        }
    }
}
