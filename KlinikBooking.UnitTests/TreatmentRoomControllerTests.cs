using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KlinikBooking.Core;
using KlinikBooking.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class TreatmentRoomsControllerTests
{
    private TreatmentRoomsController controller;
    private Mock<IRepository<TreatmentRoom>> fakeTreatmentRoomRepository;

    public TreatmentRoomsControllerTests()
    {
        var treatmentRooms = new List<TreatmentRoom>
        {
            new TreatmentRoom { Id = 1, Description = "A" },
            new TreatmentRoom { Id = 2, Description = "B" },
        };

        // Create fake TreatmentRoomRepository
        fakeTreatmentRoomRepository = new Mock<IRepository<TreatmentRoom>>();

        // Implement fake GetAllAsync() method
        fakeTreatmentRoomRepository
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(treatmentRooms);

        // Implement fake GetAsync() method for IDs 1–2
        fakeTreatmentRoomRepository.Setup(x =>
                x.GetAsync(It.IsInRange<int>(1, 2, Moq.Range.Inclusive)))
            .ReturnsAsync(treatmentRooms[1]);

        // Create TreatmentRoomsController
        controller = new TreatmentRoomsController(fakeTreatmentRoomRepository.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsListWithCorrectNumberOfTreatmentRooms()
    {
        // Act
        var result = await controller.Get() as List<TreatmentRoom>;
        var noOfTreatmentRooms = result.Count;

        // Assert
        Assert.Equal(2, noOfTreatmentRooms);
    }

    [Fact]
    public async Task GetById_TreatmentRoomExists_ReturnsIActionResultWithTreatmentRoom()
    {
        // Act
        var result = await controller.Get(2) as ObjectResult;
        var treatmentRoom = result.Value as TreatmentRoom;
        var treatmentRoomId = treatmentRoom.Id;

        // Assert
        Assert.InRange(treatmentRoomId, 1, 2);
    }

    [Fact]
    public async Task Delete_WhenIdIsLargerThanZero_RemoveIsCalled()
    {
        // Act
        await controller.Delete(1);

        // Assert
        fakeTreatmentRoomRepository.Verify(
            x => x.RemoveAsync(1),
            Times.Once
        );
    }

    [Fact]
    public async Task Delete_WhenIdIsLessThanOne_RemoveIsNotCalled()
    {
        // Act
        await controller.Delete(0);

        // Assert
        fakeTreatmentRoomRepository.Verify(
            x => x.RemoveAsync(It.IsAny<int>()),
            Times.Never
        );
    }

    [Fact]
    public async Task Delete_WhenIdIsLargerThanTwo_RemoveThrowsException()
    {
        // Arrange
        fakeTreatmentRoomRepository.Setup(x =>
                x.RemoveAsync(It.Is<int>(id => id < 1 || id > 2)))
            .Throws<InvalidOperationException>();

        Task result() => controller.Delete(3);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(result);

        fakeTreatmentRoomRepository.Verify(
            x => x.RemoveAsync(It.IsAny<int>()),
            Times.Once
        );
    }
}