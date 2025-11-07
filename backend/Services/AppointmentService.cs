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
            // Validate CategoryId
            if (a.CategoryId != 0) // Assuming 0 is not a valid CategoryId
            {
                var categoryExists = await _uow.Categories.GetByIdAsync(a.CategoryId);
                if (categoryExists == null)
                {
                    throw new ArgumentException($"Category with ID {a.CategoryId} does not exist.");
                }
            }
            else
            {
                throw new ArgumentException("CategoryId cannot be 0 or empty.");
            }


            await _uow.Appointments.AddAsync(a);
            await _uow.SaveChangesAsync();
            return a;
        }

        public async Task<Appointment?> Update(Appointment a)
        {
            // Validate CategoryId
            if (a.CategoryId != 0) // Assuming 0 is not a valid CategoryId
            {
                var categoryExists = await _uow.Categories.GetByIdAsync(a.CategoryId);
                if (categoryExists == null)
                {
                    throw new ArgumentException($"Category with ID {a.CategoryId} does not exist.");
                }
            }
            else
            {
                throw new ArgumentException("CategoryId cannot be 0 or empty.");
            }

            _uow.Appointments.Update(a);
            await _uow.SaveChangesAsync();
            return a;
        }

        public async Task<bool> Delete(int id)
        {
            var appt = await _uow.Appointments.GetByIdAsync(id);
            if (appt == null) return false;

            _uow.Appointments.Delete(appt);
            await _uow.SaveChangesAsync();
            return true;
        }
    }
}
