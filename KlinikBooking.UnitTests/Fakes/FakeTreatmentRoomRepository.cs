using System.Collections.Generic;
using System.Threading.Tasks;
using KlinikBooking.Core.Entitites;
using KlinikBooking.Core.Interfaces;


public class FakeTreatmentRoomRepository : IRepository<TreatmentRoom>
{
    // Exposed so unit tests can verify AddAsync was called
    public bool addWasCalled = false;

    public Task AddAsync(TreatmentRoom entity)
    {
        addWasCalled = true;
        return Task.CompletedTask;
    }

    // Exposed so unit tests can verify EditAsync was called
    public bool editWasCalled = false;

    public Task EditAsync(TreatmentRoom entity)
    {
        editWasCalled = true;
        return Task.CompletedTask;
    }

    public Task<TreatmentRoom> GetAsync(int id)
    {
        Task<TreatmentRoom> treatmentRoomTask =
            Task.Factory.StartNew(() =>
                new TreatmentRoom { Id = 1, Description = "A" });

        return treatmentRoomTask;
    }

    public Task<IEnumerable<TreatmentRoom>> GetAllAsync()
    {
        IEnumerable<TreatmentRoom> treatmentRooms = new List<TreatmentRoom>
        {
            new TreatmentRoom { Id = 1, Description = "A" },
            new TreatmentRoom { Id = 2, Description = "B" }
        };

        Task<IEnumerable<TreatmentRoom>> treatmentRoomsTask =
            Task.Factory.StartNew(() => treatmentRooms);

        return treatmentRoomsTask;
    }

    // Exposed so unit tests can verify RemoveAsync was called
    public bool removeWasCalled = false;

    public Task RemoveAsync(int id)
    {
        removeWasCalled = true;
        return Task.CompletedTask;
    }
}