using KlinikBooking.Core.Entitites;
using KlinikBooking.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlinikBooking.Infrastructure.Repositories
{
    public class TreatmentRoomRepository : IRepository<TreatmentRoom>
    {
        private readonly KlinikBookingContext db;
        public TreatmentRoomRepository(KlinikBookingContext context)
        {
            db = context;
        }

        public async Task AddAsync(TreatmentRoom entity)
        {
            db.TreatmentRoom.Add(entity);

            await db.SaveChangesAsync();
        }

        public Task EditAsync(TreatmentRoom entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TreatmentRoom>> GetAllAsync()
        {
            return await db.TreatmentRoom.ToListAsync();
        }

        public async Task<TreatmentRoom> GetAsync(int id)
        {
            return await db.TreatmentRoom.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            var tr = await db.TreatmentRoom.SingleAsync(r => r.Id == id);
            db.TreatmentRoom.Remove(tr);
            await db.SaveChangesAsync();
        }
    }
}
