using demoApp.Models;

namespace demoApp.Services
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAll();
        Task<Appointment?> Add(Appointment appointment);
        Task<bool> Delete(int id);
    }
}
