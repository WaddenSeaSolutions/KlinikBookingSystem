using KlinikBooking.Core.Entitites;
using KlinikBooking.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KlinikBooking.Infrastructure.Repositories
{
    public class PatientRepository : IRepository<Patient>
    {
        private readonly KlinikBookingContext db;

        public PatientRepository(KlinikBookingContext context)
        {
            db = context;
        }
        public Task AddAsync(Patient entity)
        {
            throw new NotImplementedException();
        }

        public Task EditAsync(Patient entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await db.Patient.ToListAsync();

        }

        public async Task<Patient> GetAsync(int id)
        {
            return await db.Patient.FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
