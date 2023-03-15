using Microsoft.EntityFrameworkCore;

namespace Hotel_Reservations_Manager.Model
{
    public class HotelDbContext:DbContext
    {

        public HotelDbContext(DbContextOptions options)
          : base(options)
        {

        }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Rooms> Rooms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>()
                .HasMany(c => c.Clients)
                .WithMany(r => r.Reservations)
                .HasForeignKey(s => s.ClientId)
                .UsingEntity<Dictionary<string, object>>(
                    "ClientReservation",
                    j => j.HasOne<Reservation>().WithMany().HasForeignKey("ResId"),
                    j => j.HasOne<Clients>().WithMany().HasForeignKey("ClientId"),
                    j =>
                    {
                        j.Property<int>("Id").UseIdentityColumn();
                        j.HasKey("Id");
                    });


           
        }
    }
}
