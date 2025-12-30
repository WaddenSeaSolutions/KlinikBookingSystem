using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KlinikBooking.Core;
using KlinikBooking.Core.Entitites;
using KlinikBooking.Core.Interfaces;


public class FakeBookingRepository : IRepository<Booking>
{
    private DateTime fullyOccupiedStartDate;
    private DateTime fullyOccupiedEndDate;

    public FakeBookingRepository(DateTime start, DateTime end)
    {
        fullyOccupiedStartDate = start;
        fullyOccupiedEndDate = end;
    }

    // Exposed so unit tests can verify AddAsync was called
    public bool addWasCalled = false;

    public Task AddAsync(Booking entity)
    {
        addWasCalled = true;
        return Task.CompletedTask;
    }

    // Exposed so unit tests can verify EditAsync was called
    public bool editWasCalled = false;

    public Task EditAsync(Booking entity)
    {
        editWasCalled = true;
        return Task.CompletedTask;
    }

    public Task<Booking> GetAsync(int id)
    {
        Task<Booking> bookingTask = Task.Factory.StartNew(() => new Booking
        {
            Id = 1,
            appointmentStart = fullyOccupiedStartDate,
            appointmentEnd = fullyOccupiedEndDate,
            IsActive = true,
            PatientId = 1,
            TreatmentRoomId = 1
        });

        return bookingTask;
    }

    public Task<IEnumerable<Booking>> GetAllAsync()
    {
        IEnumerable<Booking> bookings = new List<Booking>
        {
            new Booking
            {
                Id = 1,
                appointmentStart = DateTime.Today.AddDays(1),
                appointmentEnd = DateTime.Today.AddDays(1),
                IsActive = true,
                PatientId = 1,
                TreatmentRoomId = 1
            },
            new Booking
            {
                Id = 1,
                appointmentStart = fullyOccupiedStartDate,
                appointmentEnd = fullyOccupiedEndDate,
                IsActive = true,
                PatientId = 1,
                TreatmentRoomId = 1
            },
            new Booking
            {
                Id = 2,
                appointmentStart = fullyOccupiedStartDate,
                appointmentEnd = fullyOccupiedEndDate,
                IsActive = true,
                PatientId = 2,
                TreatmentRoomId = 2
            }
        };

        Task<IEnumerable<Booking>> bookingsTask =
            Task.Factory.StartNew(() => bookings);

        return bookingsTask;
    }

    // Exposed so unit tests can verify RemoveAsync was called
    public bool removeWasCalled = false;

    public Task RemoveAsync(int id)
    {
        removeWasCalled = true;
        return Task.CompletedTask;
    }
}