using backend.Interfaces;
using backend.Repositories;

namespace backend.Data
{
    public interface IUnitOfWork
    {
        IAppointmentRepository Appointments { get; }
        Task SaveAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IAppointmentRepository Appointments { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Appointments = new AppointmentRepository(context);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
