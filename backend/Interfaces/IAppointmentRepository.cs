using backend.Models;

namespace backend.Interfaces
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetUpcomingAppointments();
        Task<Appointment?> GetByIdWithUserAsync(int id);
    }
}
