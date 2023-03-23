namespace HotelReservationManager.Models
{
    public class Clients
    {
        public int ClientId { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Phone { get; set; }
        public string E_mail { get; set; }
        public bool Adult { get; set; }
        public virtual HashSet<Reservation> Reservations { get; set; }
        public Clients()
        {
            Reservations = new HashSet<Reservation>();
        }
    }
}
