namespace HotelReservationManager.Models
{
    public class LogInfo
    {
        private static bool Is_Admin;
        public bool Get() 
        {
            return Is_Admin;
        }
        public void Set(bool value)
        {
            Is_Admin = value;
        }
    }
}
