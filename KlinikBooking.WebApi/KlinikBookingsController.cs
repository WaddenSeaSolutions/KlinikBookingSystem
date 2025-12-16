using DefaultNamespace;
using Microsoft.AspNetCore.Mvc;
using KlinikBooking.Core;
using KlinikBooking.Core.Interfaces;

namespace KlinikBooking.WebApi
{
    [ApiController]
    [Route("[controller]")]
    public class KlinikBookingsController
    {

        private IRepository<Booking> bookingRepository;





    }
}
