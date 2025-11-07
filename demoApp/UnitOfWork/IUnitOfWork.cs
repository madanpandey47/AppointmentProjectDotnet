using System.Threading.Tasks;
using demoApp.Repositories;

namespace demoApp.UnitOfWork
{
    public interface IUnitOfWork
    {
        IAppointmentRepository Appointments { get; }
        Task<int> SaveAsync();
    }
}
