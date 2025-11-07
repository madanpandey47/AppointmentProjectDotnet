using backend.Data;
using backend.Models;

namespace backend.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _uow;

        public AppointmentService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<Appointment>> GetAll()
        {
            return await _uow.Appointments.GetAllAsync();
        }

        public async Task<Appointment?> Add(Appointment a)
        {
            await _uow.Appointments.AddAsync(a);
            await _uow.SaveAsync();
            return a;
        }

        public async Task<Appointment?> Update(Appointment a)
        {
            _uow.Appointments.Update(a);
            await _uow.SaveAsync();
            return a;
        }

        public async Task<bool> Delete(int id)
        {
            var appt = await _uow.Appointments.GetByIdAsync(id);
            if (appt == null) return false;

            _uow.Appointments.Delete(appt);
            await _uow.SaveAsync();
            return true;
        }
    }
}
