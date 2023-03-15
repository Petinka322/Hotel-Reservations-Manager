namespace Hotel_Reservations_Manager.Model
{
    public class Users
    {

       
        public string Password { get; set; }
        public string Username { get; set; }
        public string First_name { get; set; }
        public string Second_name { get; set; }
        public string Last_name { get; set; }
        
        public string EGN { get; set; }
        public string Phone { get; set; }
        public string E_mail { get; set; }
        public DateOnly Hire_date { get; set; }
        public bool Is_active { get; set; }
        public DateOnly Release_date { get; set; }  


    }
}
