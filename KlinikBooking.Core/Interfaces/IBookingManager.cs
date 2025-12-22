using KlinikBooking.Core.Entitites;

namespace KlinikBooking.Core.Interfaces;

public interface IBookingManager
{
    Task<bool> CreateBooking(Booking booking);
    Task<int> FindAvailableTreatmentRoom(DateTime appointmentStart, DateTime apointmentEnd);
    Task<List<DateTime>> GetFullyOccupiedTimeSlots(DateTime appointmentStart, DateTime apointmentEnd);
}