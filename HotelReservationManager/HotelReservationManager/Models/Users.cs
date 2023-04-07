namespace HotelReservationManager.Models
{
    public class Users
    {
        [Required(ErrorMessage = "Please Enter Password")]
        [Display(Name = "Please Enter Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please Enter Username")]
        [Display(Name = "Please Enter Username")]
        public string Username { get; set; }
        public string First_name { get; set; }
        public string Second_name { get; set; }
        public string Last_name { get; set; }

        public bool Is_Administrator { get; set; }
        [Key]
        public string EGN { get; set; }
        public string Phone { get; set; }
        public string E_mail { get; set; }
        public DateTime Hire_date { get; set; }
        public bool Is_active { get; set; }
        public DateTime Release_date { get; set; }
    }
}
