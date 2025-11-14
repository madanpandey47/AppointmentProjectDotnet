using backend.Repositories;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Appointment>> GetAllWithCategoryAsync()
        {
            return await _dbset
                .Include(a => a.AppointmentCategories)
                .ThenInclude(ac => ac.Category)
                .ToListAsync();
        }
        

        public async Task<Appointment?> GetByIdWithCategoryAsync(int id)
        {
            return await _dbset
                .Include(a => a.AppointmentCategories)
                .ThenInclude(ac => ac.Category)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Appointment>> GetUpcomingAppointments()
        {
            return await _dbset
                .Include(a => a.AppointmentCategories)
                .ThenInclude(ac => ac.Category)
                .Where(a => a.Date >= DateTime.Now)
                .ToListAsync();
        }
    }
}