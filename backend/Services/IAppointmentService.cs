using backend.Models;

namespace backend.Services
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAll();
        Task<Appointment?> Add(Appointment a);
        Task<Appointment?> Update(Appointment a);
        Task<bool> Delete(int id);
    }
}
