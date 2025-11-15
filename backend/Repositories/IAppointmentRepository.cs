using backend.Models;

namespace backend.Repositories
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetAll_SP();
        Task<Appointment?> GetById_SP(int id);
        Task<int> Insert_SP(Appointment appointment);
        Task<bool> Update_SP(Appointment appointment);
        Task<bool> Delete_SP(int id);
    }
}
