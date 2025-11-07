using demoApp.Data;
using demoApp.Models;

namespace demoApp.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AppDbContext context) : base(context) {}
    }
}
