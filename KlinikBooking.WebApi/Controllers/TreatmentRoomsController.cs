using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KlinikBooking.Core.Entitites;
using KlinikBooking.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KlinikBooking.WebApi
{
    [ApiController]
    [Route("[controller]")]
    public class TreatmentRoomsController : Controller
    {
        private readonly IRepository<TreatmentRoom> treatmentRoomRepository;

        public TreatmentRoomsController(IRepository<TreatmentRoom> repository)
        {
            treatmentRoomRepository = repository;
        }

        // GET: treatmentrooms
        [HttpGet(Name = "GetTreatmentRooms")]
        public async Task<IEnumerable<TreatmentRoom>> Get()
        {
            return await treatmentRoomRepository.GetAllAsync();
        }

        // GET treatmentrooms/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await treatmentRoomRepository.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return new ObjectResult(item);
        }

        // POST treatmentrooms
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TreatmentRoom treatmentRoom)
        {
            if (treatmentRoom == null)
            {
                return BadRequest();
            }

            await treatmentRoomRepository.AddAsync(treatmentRoom);
            return CreatedAtRoute("GetTreatmentRooms", null);
        }

        // DELETE treatmentrooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
            {
                await treatmentRoomRepository.RemoveAsync(id);
                return NoContent();
            }

            return BadRequest();
        }
    }
}