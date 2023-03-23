namespace HotelReservationManager.Models
{
    public class Rooms
    {
        public int RoomsId { get; set; }
        public int RoomsCapacity { get; set; }
        public string RoomsType { get; set; }

        public bool Is_Available { get; set; }
        public double Price_Adult { get; set; }
        public double Price_Child { get; set; }
    }
}
