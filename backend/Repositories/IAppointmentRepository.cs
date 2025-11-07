using backend.Models;

namespace backend.Repositories
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetUpcomingAppointments();
    }
}
