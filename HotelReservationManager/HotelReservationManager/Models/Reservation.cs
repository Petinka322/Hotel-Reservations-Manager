namespace HotelReservationManager.Models
{
    public class Reservation
    {
        public int ResId { get; set; }
        public int RoomsId { get; set; }
        public string Username { get; set; }
        public DateTime Arrival_date { get; set; }
        public DateTime Departure_date { get; set; }
        public bool Breakfast { get; set; }
        public bool All_inclusive { get; set; }
        public double Price { get; set; }
        public virtual HashSet<Clients> Clients { get; set; }
        public virtual HashSet<Users> Users { get; set; }
        public Reservation()
        {
            Clients = new HashSet<Clients>();
            Users = new HashSet<Users>();
        }
    }
}
