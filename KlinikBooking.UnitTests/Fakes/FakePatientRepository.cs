using System.Collections.Generic;
using System.Threading.Tasks;
using KlinikBooking.Core;
using KlinikBooking.Core.Entitites;
using KlinikBooking.Core.Interfaces;


public class FakePatientRepository : IRepository<Patient>
{
    // Exposed so unit tests can verify AddAsync was called
    public bool addWasCalled = false;

    public Task AddAsync(Patient entity)
    {
        addWasCalled = true;
        return Task.CompletedTask;
    }

    // Exposed so unit tests can verify EditAsync was called
    public bool editWasCalled = false;

    public Task EditAsync(Patient entity)
    {
        editWasCalled = true;
        return Task.CompletedTask;
    }

    public Task<Patient> GetAsync(int id)
    {
        Task<Patient> patientTask = Task.Factory.StartNew(() =>
            new Patient
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@example.com"
            });

        return patientTask;
    }

    public Task<IEnumerable<Patient>> GetAllAsync()
    {
        IEnumerable<Patient> patients = new List<Patient>
        {
            new Patient { Id = 1, Name = "John Doe", Email = "john@example.com" },
            new Patient { Id = 2, Name = "Jane Smith", Email = "jane@example.com" }
        };

        Task<IEnumerable<Patient>> patientsTask =
            Task.Factory.StartNew(() => patients);

        return patientsTask;
    }

    // Exposed so unit tests can verify RemoveAsync was called
    public bool removeWasCalled = false;

    public Task RemoveAsync(int id)
    {
        removeWasCalled = true;
        return Task.CompletedTask;
    }
}