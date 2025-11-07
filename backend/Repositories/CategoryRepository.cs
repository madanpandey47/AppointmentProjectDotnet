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
                .Include(c => c.Appointments)
                .FirstOrDefaultAsync(c => c.cId == id);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesWithAppointments()
        {
            return await _dbset
                .Include(c => c.Appointments)
                .ToListAsync();
        }
    }
}
