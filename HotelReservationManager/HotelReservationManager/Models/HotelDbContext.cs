
using Microsoft.EntityFrameworkCore;

namespace HotelReservationManager.Models
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions options)
         : base(options)
        {

        }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Rooms> Rooms { get; set; }

        public DbSet<ReservationClients> ReservationClients {get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(e => e.ResId);
                entity.HasMany(c => c.Clients)
                .WithMany(r => r.Reservations);
            });



            modelBuilder.Entity<Clients>(entity =>
            {
                entity.HasKey(e => e.ClientId);
                entity.HasMany(r => r.Reservations)
                .WithMany(c => c.Clients);
            });

            modelBuilder.Entity<Rooms>(entity =>
            {
                entity.HasKey(r => r.RoomsId);


            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.EGN);

            });
        }
    }
}
