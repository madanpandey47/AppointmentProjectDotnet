using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }

        public async Task<Category?> GetCategoryWithAppointments(int id)
        {
            return await _dbset
                .Include(c => c.AppointmentCategories)
                .ThenInclude(ac => ac.Appointment)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
