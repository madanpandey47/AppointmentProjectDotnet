using backend.DTOs;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDTO>> GetAll_SP();
        Task<AppointmentDTO?> GetById_SP(int id);
        Task<AppointmentDTO> Insert_SP(CreateAppointmentDTO appointmentDto, IFormFile? imageFile);
        Task<bool> Update_SP(int id, CreateAppointmentDTO appointmentDto, IFormFile? imageFile);
        Task<bool> Delete_SP(int id);
    }
}