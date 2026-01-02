using KlinikBooking.Core.Entitites;
using KlinikBooking.Core.Interfaces;

namespace KlinikBooking.Core
{
    public class BookingManager : IBookingManager
    {
        private IRepository<Booking> bookingRepository;
        private IRepository<TreatmentRoom> treatmentRoomRepository;

        // Constructor injection
        public BookingManager(IRepository<Booking> bookingRepository, IRepository<TreatmentRoom> roomRepository)
        {
            this.bookingRepository = bookingRepository;
            this.treatmentRoomRepository = roomRepository;
        }

        public async Task<bool> CreateBooking(Booking booking)
        {
            int roomId = await FindAvailableTreatmentRoom(booking.appointmentStart, booking.appointmentEnd);

            if (roomId >= 0)
            {
                booking.TreatmentRoomId = roomId;
                booking.IsActive = true;
                await bookingRepository.AddAsync(booking);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<int> FindAvailableTreatmentRoom(DateTime appointmentStart, DateTime appointmentEnd)
        {
            if (appointmentStart >= appointmentEnd)
                throw new ArgumentException("Appointments must have valid start time and end time");


            TimeSpan duration = appointmentEnd - appointmentStart;
            if (duration.TotalHours > 1)
            {
                throw new ArgumentException("Appointments can be max 1 hour");
            }

            var rooms = await treatmentRoomRepository.GetAllAsync();
            var bookings = await bookingRepository.GetAllAsync();
            var activeBookings = bookings.Where(b => b.IsActive).ToList();

            foreach (var room in rooms)
            {

                bool isOccupied = activeBookings.Any(b =>
                    b.TreatmentRoomId == room.Id &&
                    appointmentStart < b.appointmentEnd &&
                    appointmentEnd > b.appointmentStart);

                if (!isOccupied)
                {
                    return room.Id;
                }
            }

            return -1;
        }

        public async Task<List<DateTime>> GetFullyOccupiedTimeSlots(DateTime apointmentStart, DateTime apointmentEnd)
        {
            if (apointmentStart > apointmentEnd)
                throw new ArgumentException("The start date cannot be later than the end date.");

            List<DateTime> fullyOccupiedSlots = new List<DateTime>();

            var rooms = await treatmentRoomRepository.GetAllAsync();
            int noOfTreatmentRooms = rooms.Count();

            if (noOfTreatmentRooms == 0) 
            {
                return new List<DateTime>();
            }

            var bookings = (await bookingRepository.GetAllAsync()).Where(b => b.IsActive).ToList();

            for (DateTime slot = apointmentStart; slot < apointmentEnd; slot = slot.AddHours(1))
            {
                var slotEnd = slot.AddHours(1);

                var activeBookingsInSlot = bookings.Count(b =>
                    b.appointmentStart < slotEnd && b.appointmentEnd > slot);

                if (activeBookingsInSlot >= noOfTreatmentRooms)
                {
                    fullyOccupiedSlots.Add(slot);
                }
            }

            return fullyOccupiedSlots;
        }

    }
}
