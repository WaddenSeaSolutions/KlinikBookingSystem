using System.Collections.Generic;
using System.Threading.Tasks;
using KlinikBooking.Core.Entitites;
using KlinikBooking.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KlinikBooking.WebApi
{
    [ApiController]
    [Route("[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IRepository<Patient> patientRepository;

        public PatientsController(IRepository<Patient> repository)
        {
            patientRepository = repository;
        }

        // GET: patients
        [HttpGet]
        public async Task<IEnumerable<Patient>> Get()
        {
            return await patientRepository.GetAllAsync();
        }
    }
}