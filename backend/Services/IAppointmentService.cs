using backend.DTOs;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDTO>> GetAllAppointmentsAsync();
        Task<AppointmentDTO?> GetAppointmentByIdAsync(int id);
        Task<AppointmentDTO> CreateAppointmentAsync(CreateAppointmentDTO appointmentDto, IFormFile? imageFile);
        Task<bool> UpdateAppointmentAsync(int id, CreateAppointmentDTO appointmentDto, IFormFile? imageFile);
        Task<bool> DeleteAppointmentAsync(int id);
    }
}