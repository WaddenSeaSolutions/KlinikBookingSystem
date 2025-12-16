using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KlinikBooking.Core;
using KlinikBooking.Core.Entitites;
using KlinikBooking.Core.Interfaces;
using KlinikBooking.WebApi.Controllers;
using Moq;
using Xunit;

public class PatientsControllerTests
{
    private PatientsController controller;
    private Mock<IRepository<Patient>> fakePatientRepository;

    public PatientsControllerTests()
    {
        fakePatientRepository = new Mock<IRepository<Patient>>();
        controller = new PatientsController(fakePatientRepository.Object);
    }

    // Test data for different patient scenarios
    public static IEnumerable<object[]> GetPatientTestData()
    {
        // Empty list
        yield return new object[]
        {
            new List<Patient>(),
            0,
            "Empty patient list"
        };

        // Single patient
        yield return new object[]
        {
            new List<Patient>
            {
                new Patient
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "john@example.com"
                }
            },
            1,
            "Single patient"
        };

        // Multiple patients
        yield return new object[]
        {
            new List<Patient>
            {
                new Patient { Id = 1, Name = "John Doe", Email = "john@example.com" },
                new Patient { Id = 2, Name = "Jane Smith", Email = "jane@example.com" },
                new Patient { Id = 3, Name = "Bob Johnson", Email = "bob@example.com" }
            },
            3,
            "Multiple patients"
        };

        // Large patient list
        yield return new object[]
        {
            Enumerable.Range(1, 10)
                .Select(i => new Patient
                {
                    Id = i,
                    Name = $"Patient {i}",
                    Email = $"patient{i}@example.com"
                })
                .ToList(),
            10,
            "Large patient list"
        };
    }
}