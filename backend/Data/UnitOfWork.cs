using backend.Data;
using backend.Repositories;
using System;
using System.Threading.Tasks;

namespace backend.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;
        public IAppointmentRepository Appointments { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Appointments = new AppointmentRepository(context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
