using KlinikBooking.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using KlinikBooking.Core.Entitites;
using System;

namespace KlinikBooking.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KlinikBookingsController : ControllerBase
    {

        private IRepository<Booking> bookingRepository;
        private IBookingManager bookingManager;

        public KlinikBookingsController(IRepository<Booking> bookingRepository, IBookingManager bookingManager)
        {
            this.bookingRepository = bookingRepository;
            this.bookingManager = bookingManager;
        }

        [HttpGet]
        [Route("GetFullyOccupiedSlots")]
        public async Task<ActionResult<List<DateTime>>> GetFullyOccupiedSlots([FromQuery] DateTime appointmentStart, [FromQuery] DateTime apointmentEnd)
        {
            try
            {
                var occupiedSlots = await bookingManager.GetFullyOccupiedTimeSlots(appointmentStart, apointmentEnd);
                return Ok(occupiedSlots);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllBookings")]
        public async Task<IEnumerable<Booking>> Get()
        {
            return await bookingRepository.GetAllAsync();
        }

        [HttpPost]
        [Route("CreateBooking")]
        public async Task<IActionResult> CreateBooking([FromBody] Booking booking)
        {
            if (booking == null)
            {
                return BadRequest();
            }

            bool created = await bookingManager.CreateBooking(booking);

            if (created)
            {
                return Created(string.Empty, booking);
            }
            else
            {
                return Conflict("The booking could not be created. All rooms are occupied. Please try another period.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await bookingRepository.GetAsync(id) == null)
            {
                return NotFound();
            }

            await bookingRepository.RemoveAsync(id);
            return NoContent();
        }

    }
}
