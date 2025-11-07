using backend.Repositories;
using System;
using System.Threading.Tasks;

namespace backend.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IAppointmentRepository Appointments { get; }
        Task<int> SaveChangesAsync();
    }
}