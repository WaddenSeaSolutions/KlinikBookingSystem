using Microsoft.EntityFrameworkCore;

namespace KlinikBooking.Infrastructure.Repositories;

public class KlinikBookingContext : DbContext
{
    public KlinikBookingContext(DbContextOptions<KlinikBookingContext> options)
        : base(options)
    {
    }

    public DbSet<Booking> Booking { get; set; }

    public DbSet<TreatmentRoom> TreatmentRoom { get; set; }

    public DbSet<Patient> Patient { get; set; }
}