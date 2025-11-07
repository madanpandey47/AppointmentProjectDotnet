using backend.Interfaces;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Appointment>> GetUpcomingAppointments()
        {
            return await _dbset
                .Include(a => a.User) // Include user details
                .Where(a => a.Date >= DateTime.Now)
                .ToListAsync();
        }

        public async Task<Appointment?> GetByIdWithUserAsync(int id)
        {
            return await _dbset
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
