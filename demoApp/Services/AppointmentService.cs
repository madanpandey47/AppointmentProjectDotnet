using demoApp.Models;
using demoApp.UnitOfWork;

namespace demoApp.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _uow;

        public AppointmentService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<Appointment>> GetAll() =>
            await _uow.Appointments.GetAllAsync();

        public async Task<Appointment?> Add(Appointment appointment)
        {
            await _uow.Appointments.AddAsync(appointment);
            await _uow.SaveAsync();
            return appointment;
        }

        public async Task<bool> Delete(int id)
        {
            var appointment = await _uow.Appointments.GetByIdAsync(id);
            if (appointment == null) return false;

            _uow.Appointments.Delete(appointment);
            await _uow.SaveAsync();
            return true;
        }
    }
}
