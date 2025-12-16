namespace KlinikBooking.Infrastructure.Repositories;

public class Dbinitializer : IDbinitializer
{
    // This method will create and seed the database.
    public void Initialize(KlinikBookingContext context)
    {
        // Delete the database, if it already exists. I do this because an
        // existing database may not be compatible with the entity model,
        // if the entity model was changed since the database was created.
        context.Database.EnsureDeleted();

        // Create the database, if it does not already exists. This operation
        // is necessary, if you don't use the in-memory database.
        context.Database.EnsureCreated();

        // Look for any bookings.
        if (context.Booking.Any())
        {
            return;   // DB has been seeded
        }

        List<Patient> patients = new List<Patient>
        {
            new Patient { Name="John Smith", Email="js@gmail.com" },
            new Patient { Name="Jane Doe", Email="jd@gmail.com" }
        };

        List<TreatmentRoom> treatmentRooms = new List<TreatmentRoom>
        {
            new TreatmentRoom { Description="A" },
            new TreatmentRoom { Description="B" },
            new TreatmentRoom { Description="C" }
        };

        DateTime date = DateTime.Today.AddDays(4);
        List<Booking> bookings = new List<Booking>
        {
            new Booking { StartDate=date, EndDate=date.AddDays(14), IsActive=true, PatientId=1, TreatmentRoomId=1 },
            new Booking { StartDate=date, EndDate=date.AddDays(14), IsActive=true, PatientId=2, TreatmentRoomId=2 },
            new Booking { StartDate=date, EndDate=date.AddDays(14), IsActive=true, PatientId=1, TreatmentRoomId=3 }
        };

        context.Patient.AddRange(patients);
        context.TreatmentRoom.AddRange(treatmentRooms);
        context.SaveChanges();
        context.Booking.AddRange(bookings);
        context.SaveChanges();
    }
}
}