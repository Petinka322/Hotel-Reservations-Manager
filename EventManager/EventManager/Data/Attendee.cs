namespace EventManager.Data
{
    public class Attendee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public virtual HashSet<Event> Events { get; set; }

        public Attendee()
        {
            Events = new HashSet<Event>();
        }
    }
}
