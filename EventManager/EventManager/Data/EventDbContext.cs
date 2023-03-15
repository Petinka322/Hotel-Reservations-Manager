using Microsoft.EntityFrameworkCore;

namespace EventManager.Data
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions options)
            : base(options)
        {

        }
        public DbSet<Event> Events { get; set; }
        public DbSet<Attendee> Attendees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Attendees)
                .WithMany(a => a.Events)
                .UsingEntity<Dictionary<string, object>>(
                    "EventAttendee",
                    j => j.HasOne<Attendee>().WithMany().HasForeignKey("AttendeeId"),
                    j => j.HasOne<Event>().WithMany().HasForeignKey("EventId"),
                    j =>
                    {
                        j.Property<int>("Id").UseIdentityColumn();
                        j.HasKey("Id");
                    });
        }
    }
}
