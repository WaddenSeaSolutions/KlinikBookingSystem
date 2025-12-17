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
            int roomId = await FindAvailableTreatmentRoom(booking.StartDate, booking.EndDate);

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

        public async Task<int> FindAvailableTreatmentRoom(DateTime startDate, DateTime endDate)
        {
            if (startDate <= DateTime.Today || startDate > endDate)
                throw new ArgumentException("The start date cannot be in the past or later than the end date.");

            var bookings = await bookingRepository.GetAllAsync();
            var activeBookings = bookings.Where(b => b.IsActive);
            var rooms = await treatmentRoomRepository.GetAllAsync();
            foreach (var room in rooms)
            {
                var activeBookingsForCurrentRoom = activeBookings.Where(b => b.TreatmentRoomId == room.Id);
                if (activeBookingsForCurrentRoom.All(b => startDate < b.StartDate &&
                    endDate < b.StartDate || startDate > b.EndDate && endDate > b.EndDate))
                {
                    return room.Id;
                }
            }
            return -1;
        }

        public async Task<List<DateTime>> GetFullyOccupiedDates(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new ArgumentException("The start date cannot be later than the end date.");

            List<DateTime> fullyOccupiedDates = new List<DateTime>();
            var rooms = await treatmentRoomRepository.GetAllAsync();
            int noOfRooms = rooms.Count();
            var bookings = await bookingRepository.GetAllAsync();

            if (bookings.Any())
            {
                for (DateTime d = startDate; d <= endDate; d = d.AddDays(1))
                {
                    var noOfBookings = from b in bookings
                                       where b.IsActive && d >= b.StartDate && d <= b.EndDate
                                       select b;
                    if (noOfBookings.Count() >= noOfRooms)
                        fullyOccupiedDates.Add(d);
                }
            }
            return fullyOccupiedDates;
        }

    }
}
