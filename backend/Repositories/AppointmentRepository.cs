using backend.Data;
using backend.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all appointments
        public async Task<IEnumerable<Appointment>> GetAll_SP()
        {
            var appointments = await _context.Appointments
                .FromSqlRaw("EXEC mp_GetAllAppointments")
                .ToListAsync();

            foreach (var ap in appointments)
            {
                ap.AppointmentCategories = await _context.AppointmentCategories
                    .Where(x => x.AppointmentId == ap.Id)
                    .Include(x => x.Category)
                    .ToListAsync();
            }

            return appointments;
        }

        // Get by Id
        public async Task<Appointment?> GetById_SP(int id)
        {
            var param = new SqlParameter("@Id", id);

            var data = await _context.Appointments
                .FromSqlRaw("EXEC mp_GetAppointmentById @Id", param)
                .ToListAsync();

            var appointment = data.FirstOrDefault();
            if (appointment == null) return null;

            appointment.AppointmentCategories = await _context.AppointmentCategories
                .Where(x => x.AppointmentId == appointment.Id)
                .Include(x => x.Category)
                .ToListAsync();

            return appointment;
        }

        // Insert appointment + categories
        public async Task<int> Insert_SP(Appointment appointment)
        {
            var newId = new SqlParameter("@NewId", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC mp_InsertAppointment @Title, @Description, @Date, @Image, @NewId OUTPUT",
                new SqlParameter("@Title", appointment.Title ?? (object)DBNull.Value),
                new SqlParameter("@Description", appointment.Description ?? (object)DBNull.Value),
                new SqlParameter("@Date", appointment.Date),
                new SqlParameter("@Image", appointment.Image ?? (object)DBNull.Value),
                newId
            );

            // Categories via CSV
            var csv = string.Join(",", appointment.AppointmentCategories.Select(c => c.CategoryId));
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC mp_UpdateAppointmentCategories @AppointmentId, @CategoryCsv",
                new SqlParameter("@AppointmentId", (int)newId.Value),
                new SqlParameter("@CategoryCsv", csv)
            );

            return (int)newId.Value;
        }

        // Update appointment + categories
        public async Task<bool> Update_SP(Appointment appointment)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC mp_UpdateAppointment @Id, @Title, @Description, @Date, @Image",
                new SqlParameter("@Id", appointment.Id),
                new SqlParameter("@Title", appointment.Title ?? (object)DBNull.Value),
                new SqlParameter("@Description", appointment.Description ?? (object)DBNull.Value),
                new SqlParameter("@Date", appointment.Date),
                new SqlParameter("@Image", appointment.Image ?? (object)DBNull.Value)
            );

            // Categories in one call
            var csv = string.Join(",", appointment.AppointmentCategories.Select(c => c.CategoryId));
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC mp_UpdateAppointmentCategories @AppointmentId, @CategoryCsv",
                new SqlParameter("@AppointmentId", appointment.Id),
                new SqlParameter("@CategoryCsv", csv)
            );

            return true;
        }

        // Delete SP
        public async Task<bool> Delete_SP(int id)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC mp_DeleteAppointment @Id",
                new SqlParameter("@Id", id)
            );

            return true;
        }

        // Paginated list
        public async Task<(IEnumerable<Appointment> appointments, int totalCount)> GetAllAsync(int page, int size)
        {
            var query = _context.Appointments
                .Include(a => a.AppointmentCategories)
                    .ThenInclude(ac => ac.Category);

            var total = await query.CountAsync();
            var data = await query.Skip((page - 1) * size).Take(size).ToListAsync();

            return (data, total);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Appointments.CountAsync();
        }
    }
}
