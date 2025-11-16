using System.Linq.Expressions;
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

        public async Task<IEnumerable<Appointment>> GetAll_SP()
        {
            var appointments = await _context.Appointments
                .FromSqlRaw("EXEC mp_GetAllAppointments")
                .ToListAsync();

            foreach (var appointment in appointments)
            {
                appointment.AppointmentCategories = await _context.AppointmentCategories
                    .Where(ac => ac.AppointmentId == appointment.Id)
                    .Include(ac => ac.Category)
                    .ToListAsync();
            }

            return appointments;
        }

        public async Task<Appointment?> GetById_SP(int id)
        {
            var param = new SqlParameter("@Id", id);
            var appointments = await _context.Appointments
                .FromSqlRaw("EXEC mp_GetAppointmentById @Id", param)
                .ToListAsync();

            var appointment = appointments.FirstOrDefault();
            if (appointment == null) return null;

            appointment.AppointmentCategories = await _context.AppointmentCategories
                .Where(ac => ac.AppointmentId == appointment.Id)
                .Include(ac => ac.Category)
                .ToListAsync();

            return appointment;
        }

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

            foreach (var ac in appointment.AppointmentCategories)
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC mp_InsertAppointmentCategory @AppointmentId, @CategoryId",
                    new SqlParameter("@AppointmentId", newId.Value),
                    new SqlParameter("@CategoryId", ac.CategoryId)
                );
            }

            return (int)newId.Value;
        }

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

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC mp_DeleteAppointmentCategoriesByAppointmentId @AppointmentId",
                new SqlParameter("@AppointmentId", appointment.Id)
            );

            foreach (var ac in appointment.AppointmentCategories)
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC mp_InsertAppointmentCategory @AppointmentId, @CategoryId",
                    new SqlParameter("@AppointmentId", appointment.Id),
                    new SqlParameter("@CategoryId", ac.CategoryId)
                );
            }

            return true;
        }

        public async Task<bool> Delete_SP(int id)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC mp_DeleteAppointment @Id",
                new SqlParameter("@Id", id)
            );

            return true;
        }

        public Task<IEnumerable<Appointment>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Appointment?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(Appointment entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Appointment entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Appointment entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Appointment>> FindAsync(Expression<Func<Appointment, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
