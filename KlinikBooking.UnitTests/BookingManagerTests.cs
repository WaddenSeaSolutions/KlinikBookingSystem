using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KlinikBooking.Core;
using KlinikBooking.Core.Entitites;
using KlinikBooking.Core.Interfaces;
using Moq;
using Xunit;

public class BookingManagerTests
{
    private readonly Mock<IRepository<Booking>> _mockBookingRepository;
    private readonly Mock<IRepository<TreatmentRoom>> _mockTreatmentRoomRepository;
    private readonly IBookingManager _bookingManager;

    public BookingManagerTests()
    {
        _mockBookingRepository = new Mock<IRepository<Booking>>();
        _mockTreatmentRoomRepository = new Mock<IRepository<TreatmentRoom>>();
        _bookingManager = new BookingManager(
            _mockBookingRepository.Object,
            _mockTreatmentRoomRepository.Object
        );
    }

    [Fact]
    public async Task FindAvailableTreatmentRoom_StartDateNotInTheFuture_ThrowsArgumentException()
    {
        // Arrange
        DateTime date = DateTime.Today;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _bookingManager.FindAvailableTreatmentRoom(date, date));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(30)]
    public async Task FindAvailableTreatmentRoom_TreatmentRoomAvailable_ReturnsValidTreatmentRoomId(int daysFromToday)
    {
        // Arrange
        DateTime date = DateTime.Today.AddDays(daysFromToday);

        var availableTreatmentRooms = new List<TreatmentRoom>
        {
            new TreatmentRoom { Id = 1, Description = "Room 1" },
            new TreatmentRoom { Id = 2, Description = "Room 2" }
        };

        var existingBookings = new List<Booking>();

        _mockTreatmentRoomRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(availableTreatmentRooms);

        _mockBookingRepository.Setup(b => b.GetAllAsync())
            .ReturnsAsync(existingBookings);

        // Act
        int treatmentRoomId =
            await _bookingManager.FindAvailableTreatmentRoom(date, date);

        // Assert
        Assert.True(treatmentRoomId > 0);
        _mockTreatmentRoomRepository.Verify(r => r.GetAllAsync(), Times.Once);
        _mockBookingRepository.Verify(b => b.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task FindAvailableTreatmentRoom_NoTreatmentRoomsAvailable_ReturnsMinusOne()
    {
        // Arrange
        DateTime date = DateTime.Today.AddDays(1);

        var treatmentRooms = new List<TreatmentRoom>
        {
            new TreatmentRoom { Id = 1, Description = "Room 1" }
        };

        var bookings = new List<Booking>
        {
            new Booking
            {
                Id = 1,
                TreatmentRoomId = 1,
                StartDate = date,
                EndDate = date,
                IsActive = true
            }
        };

        _mockTreatmentRoomRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(treatmentRooms);

        _mockBookingRepository.Setup(b => b.GetAllAsync())
            .ReturnsAsync(bookings);

        // Act
        int treatmentRoomId =
            await _bookingManager.FindAvailableTreatmentRoom(date, date);

        // Assert
        Assert.Equal(-1, treatmentRoomId);
    }

    [Theory]
    [InlineData(1, 2, true)]
    [InlineData(5, 6, true)]
    [InlineData(1, 1, true)]
    public async Task CreateBooking_TreatmentRoomAvailable_ReturnsExpectedResult(
        int startDaysFromToday,
        int endDaysFromToday,
        bool expectedResult)
    {
        // Arrange
        var booking = new Booking
        {
            StartDate = DateTime.Today.AddDays(startDaysFromToday),
            EndDate = DateTime.Today.AddDays(endDaysFromToday)
        };

        var availableTreatmentRooms = new List<TreatmentRoom>
        {
            new TreatmentRoom { Id = 1, Description = "Available Room" }
        };

        var existingBookings = new List<Booking>();

        _mockTreatmentRoomRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(availableTreatmentRooms);

        _mockBookingRepository.Setup(b => b.GetAllAsync())
            .ReturnsAsync(existingBookings);

        _mockBookingRepository.Setup(b => b.AddAsync(It.IsAny<Booking>()))
            .Returns(Task.CompletedTask);

        // Act
        bool result = await _bookingManager.CreateBooking(booking);

        // Assert
        Assert.Equal(expectedResult, result);

        if (expectedResult)
        {
            Assert.True(booking.TreatmentRoomId > 0);
            Assert.True(booking.IsActive);
            _mockBookingRepository.Verify(b => b.AddAsync(booking), Times.Once);
        }
    }

    [Theory]
    [InlineData(10, 15, false)]
    [InlineData(12, 18, false)]
    public async Task CreateBooking_NoTreatmentRoomAvailable_ReturnsFalse(
        int startDaysFromToday,
        int endDaysFromToday,
        bool expectedResult)
    {
        // Arrange
        var booking = new Booking
        {
            StartDate = DateTime.Today.AddDays(startDaysFromToday),
            EndDate = DateTime.Today.AddDays(endDaysFromToday)
        };

        var treatmentRooms = new List<TreatmentRoom>
        {
            new TreatmentRoom { Id = 1, Description = "Room 1" }
        };

        var existingBookings = new List<Booking>
        {
            new Booking
            {
                Id = 1,
                TreatmentRoomId = 1,
                StartDate = DateTime.Today.AddDays(10),
                EndDate = DateTime.Today.AddDays(20),
                IsActive = true
            }
        };

        _mockTreatmentRoomRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(treatmentRooms);

        _mockBookingRepository.Setup(b => b.GetAllAsync())
            .ReturnsAsync(existingBookings);

        // Act
        bool result = await _bookingManager.CreateBooking(booking);

        // Assert
        Assert.Equal(expectedResult, result);
        _mockBookingRepository.Verify(
            b => b.AddAsync(It.IsAny<Booking>()),
            Times.Never
        );
    }

    [Fact]
    public async Task CreateBooking_NoTreatmentRoomAvailable_DoesNotModifyBookingProperties()
    {
        // Arrange
        var booking = new Booking
        {
            StartDate = DateTime.Today.AddDays(15),
            EndDate = DateTime.Today.AddDays(16),
            TreatmentRoomId = 999,
            IsActive = true
        };

        var treatmentRooms = new List<TreatmentRoom>
        {
            new TreatmentRoom { Id = 1 }
        };

        var conflictingBookings = new List<Booking>
        {
            new Booking
            {
                TreatmentRoomId = 1,
                StartDate = DateTime.Today.AddDays(10),
                EndDate = DateTime.Today.AddDays(20),
                IsActive = true
            }
        };

        _mockTreatmentRoomRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(treatmentRooms);

        _mockBookingRepository.Setup(b => b.GetAllAsync())
            .ReturnsAsync(conflictingBookings);

        // Act
        bool result = await _bookingManager.CreateBooking(booking);

        // Assert
        Assert.False(result);
        Assert.Equal(999, booking.TreatmentRoomId);
        Assert.True(booking.IsActive);
    }

    public static IEnumerable<object[]> GetBookingScenarios()
    {
        yield return new object[] { 1, 2, true, "Available future dates" };
        yield return new object[] { 25, 26, true, "Available dates after conflicts" };
        yield return new object[] { 15, 16, false, "Conflicting dates" };
    }

    [Theory]
    [MemberData(nameof(GetBookingScenarios))]
    public async Task CreateBooking_VariousScenarios_ReturnsExpectedResult(
        int startDaysFromToday,
        int endDaysFromToday,
        bool expectedResult,
        string scenario)
    {
        // Arrange
        var booking = new Booking
        {
            StartDate = DateTime.Today.AddDays(startDaysFromToday),
            EndDate = DateTime.Today.AddDays(endDaysFromToday)
        };

        var treatmentRooms = new List<TreatmentRoom>
        {
            new TreatmentRoom { Id = 1 }
        };

        var existingBookings = expectedResult
            ? new List<Booking>()
            : new List<Booking>
            {
                new Booking
                {
                    TreatmentRoomId = 1,
                    StartDate = DateTime.Today.AddDays(10),
                    EndDate = DateTime.Today.AddDays(20),
                    IsActive = true
                }
            };

        _mockTreatmentRoomRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(treatmentRooms);

        _mockBookingRepository.Setup(b => b.GetAllAsync())
            .ReturnsAsync(existingBookings);

        _mockBookingRepository.Setup(b => b.AddAsync(It.IsAny<Booking>()))
            .Returns(Task.CompletedTask);

        // Act
        bool result = await _bookingManager.CreateBooking(booking);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task GetFullyOccupiedDates_StartDateLaterThanEndDate_ThrowsArgumentException()
    {
        // Arrange
        DateTime startDate = DateTime.Today.AddDays(2);
        DateTime endDate = DateTime.Today.AddDays(1);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _bookingManager.GetFullyOccupiedTimeSlots(startDate, endDate));
    }

    [Fact]
    public async Task GetFullyOccupiedDates_NoTreatmentRoomsExist_ReturnsEmptyList()
    {
        // Arrange
        DateTime startDate = DateTime.Today;
        DateTime endDate = DateTime.Today.AddDays(5);

        _mockTreatmentRoomRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<TreatmentRoom>());

        // Act
        var result =
            await _bookingManager.GetFullyOccupiedTimeSlots(startDate, endDate);

        // Assert
        Assert.Empty(result);
        _mockTreatmentRoomRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetFullyOccupiedDates_NoBookingsExist_ReturnsEmptyList()
    {
        // Arrange
        DateTime startDate = DateTime.Today;
        DateTime endDate = DateTime.Today.AddDays(5);

        _mockTreatmentRoomRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<TreatmentRoom> { new TreatmentRoom { Id = 1 } });

        _mockBookingRepository.Setup(b => b.GetAllAsync())
            .ReturnsAsync(new List<Booking>());

        // Act
        var result =
            await _bookingManager.GetFullyOccupiedTimeSlots(startDate, endDate);

        // Assert
        Assert.Empty(result);
        _mockBookingRepository.Verify(b => b.GetAllAsync(), Times.Once);
    }
}