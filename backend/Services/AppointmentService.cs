using backend.DTOs;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Http;

namespace backend.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;
        private readonly IWebHostEnvironment _hosting;

        public AppointmentService(IAppointmentRepository repo, IWebHostEnvironment hosting)
        {
            _repo = repo;
            _hosting = hosting;
        }

        public async Task<IEnumerable<AppointmentDTO>> GetAll_SP()
        {
            var appointments = await _repo.GetAll_SP();
            return appointments.Select(a => new AppointmentDTO
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Date = a.Date,
                Image = a.Image,
                Categories = a.AppointmentCategories.Select(ac => new CategoryDTO
                {
                    Id = ac.CategoryId,
                    Name = ac.Category?.Name ?? ""
                }).ToList()
            });
        }

        public async Task<AppointmentDTO?> GetById_SP(int id)
        {
            var appointment = await _repo.GetById_SP(id);
            if (appointment == null) return null;

            return new AppointmentDTO
            {
                Id = appointment.Id,
                Title = appointment.Title,
                Description = appointment.Description,
                Date = appointment.Date,
                Image = appointment.Image,
                Categories = appointment.AppointmentCategories.Select(ac => new CategoryDTO
                {
                    Id = ac.CategoryId,
                    Name = ac.Category?.Name ?? ""
                }).ToList()
            };
        }

        public async Task<AppointmentDTO> Insert_SP(CreateAppointmentDTO dto, IFormFile? imageFile)
        {
            var appointment = new Appointment
            {
                Title = dto.Title,
                Description = dto.Description,
                Date = dto.Date,
                AppointmentCategories = dto.CategoryIds.Select(cid => new AppointmentCategory { CategoryId = cid }).ToList()
            };

            if (imageFile != null)
                appointment.Image = await SaveImage(imageFile);

            appointment.Id = await _repo.Insert_SP(appointment);

            return new AppointmentDTO
            {
                Id = appointment.Id,
                Title = appointment.Title,
                Description = appointment.Description,
                Date = appointment.Date,
                Image = appointment.Image,
                Categories = appointment.AppointmentCategories.Select(ac => new CategoryDTO { Id = ac.CategoryId }).ToList()
            };
        }

        public async Task<bool> Update_SP(int id, CreateAppointmentDTO dto, IFormFile? imageFile)
        {
            var appointment = new Appointment
            {
                Id = id,
                Title = dto.Title,
                Description = dto.Description,
                Date = dto.Date,
                AppointmentCategories = dto.CategoryIds.Select(cid => new AppointmentCategory { CategoryId = cid }).ToList()
            };

            if (imageFile != null)
                appointment.Image = await SaveImage(imageFile);

            return await _repo.Update_SP(appointment);
        }

        public async Task<bool> Delete_SP(int id)
        {
            return await _repo.Delete_SP(id);
        }

        private async Task<string> SaveImage(IFormFile file)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var path = Path.Combine(_hosting.WebRootPath, "uploads", fileName);
            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
            return $"/uploads/{fileName}";
        }
    }
}
