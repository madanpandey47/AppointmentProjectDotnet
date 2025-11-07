using demoApp.Data;
using demoApp.Repositories;

namespace demoApp.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IAppointmentRepository Appointments { get; }

        public UnitOfWork(AppDbContext context, IAppointmentRepository appointmentRepo)
        {
            _context = context;
            Appointments = appointmentRepo;
        }

        public async Task<int> SaveAsync() =>
            await _context.SaveChangesAsync();
    }
}
