using KlinikBooking.Core.Entitites;

namespace KlinikBooking.Core.Interfaces;

public interface IBookingManager
{
    Task<bool> CreateBooking(Booking booking);
    Task<int> FindAvailableTreatmentRoom(DateTime startDate, DateTime endDate);
    Task<List<DateTime>> GetFullyOccupiedDates(DateTime startDate, DateTime endDate);
}