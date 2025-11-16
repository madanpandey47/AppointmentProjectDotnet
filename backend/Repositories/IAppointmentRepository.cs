using backend.Models;

namespace backend.Repositories
{
    public interface IAppointmentRepository 
    {
        Task<IEnumerable<Appointment>> GetAll_SP();
        Task<Appointment?> GetById_SP(int id);
        Task<int> Insert_SP(Appointment appointment);
        Task<bool> Update_SP(Appointment appointment);
        Task<bool> Delete_SP(int id);
        Task<(IEnumerable<Appointment> appointments, int totalCount)> GetAllAsync(int pageNumber, int pageSize);
        Task<int> CountAsync();
    }
}
