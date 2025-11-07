using System;
using System.Threading.Tasks;

namespace backend.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAppointmentRepository Appointments { get; }
        Task<int> SaveChangesAsync();
    }
}