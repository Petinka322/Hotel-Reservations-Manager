namespace EventManager.Data
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
         Location { get; set; }
        public string Description {
        public virtual HashSet<Attendee> Attendees { get; set; }

        public Event()
        {
            Attendees = new HashSet<Attendee>();
        }
    }
}
