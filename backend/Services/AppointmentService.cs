using backend.Data;
using backend.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly backend.Data.IUnitOfWork _uow;
        private readonly IWebHostEnvironment _hosting;

        public AppointmentService(backend.Data.IUnitOfWork uow, IWebHostEnvironment hosting)
        {
            _uow = uow;
            _hosting = hosting;
        }

        public async Task<IEnumerable<AppointmentDTO>> GetAllAppointmentsAsync()
        {
            var data = await _uow.Appointments.GetAllWithCategoryAsync();
            return data.Select(a => new AppointmentDTO
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Date = a.Date,
                Image = a.Image,
                Categories = a.AppointmentCategories.Select(ac => new CategoryDTO { Id = ac.CategoryId, Name = ac.Category.Name }).ToList()
            });
        }

        public async Task<AppointmentDTO?> GetAppointmentByIdAsync(int id)
        {
            var appointment = await _uow.Appointments.GetByIdWithCategoryAsync(id);
            if (appointment == null) return null;

            return new AppointmentDTO
            {
                Id = appointment.Id,
                Title = appointment.Title,
                Description = appointment.Description,
                Date = appointment.Date,
                Image = appointment.Image,
                Categories = appointment.AppointmentCategories.Select(ac => new CategoryDTO { Id = ac.CategoryId, Name = ac.Category.Name }).ToList()
            };
        }

        public async Task<AppointmentDTO> CreateAppointmentAsync(CreateAppointmentDTO appointmentDto, IFormFile imageFile)
        {
            var appointment = new Appointment
            {
                Title = appointmentDto.Title,
                Description = appointmentDto.Description,
                Date = appointmentDto.Date,
                AppointmentCategories = new List<AppointmentCategory>()
            };

            if (imageFile != null)
            {
                appointment.Image = await SaveImage(imageFile);
            }

            foreach (var categoryId in appointmentDto.CategoryIds)
            {
                appointment.AppointmentCategories.Add(new AppointmentCategory { CategoryId = categoryId });
            }

            await _uow.Appointments.AddAsync(appointment);
            await _uow.SaveChangesAsync();

            return new AppointmentDTO { Id = appointment.Id, Title = appointment.Title, Description = appointment.Description, Date = appointment.Date, Image = appointment.Image };
        }

        public async Task<bool> UpdateAppointmentAsync(int id, CreateAppointmentDTO appointmentDto, IFormFile? imageFile)
        {
            var existing = await _uow.Appointments.GetByIdWithCategoryAsync(id);
            if (existing == null) return false;

            existing.Title = appointmentDto.Title;
            existing.Description = appointmentDto.Description;
            existing.Date = appointmentDto.Date;

            if (imageFile != null)
            {
                if (!string.IsNullOrEmpty(existing.Image))
                {
                    DeleteImage(existing.Image);
                }
                existing.Image = await SaveImage(imageFile);
            }

            existing.AppointmentCategories.Clear();
            foreach (var categoryId in appointmentDto.CategoryIds)
            {
                existing.AppointmentCategories.Add(new AppointmentCategory { CategoryId = categoryId });
            }

            _uow.Appointments.Update(existing);
            await _uow.SaveChangesAsync();
            return true;
        }

                private async Task<string> SaveImage(IFormFile imageFile)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            var filePath = Path.Combine(_hosting.WebRootPath, "uploads", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/uploads/{fileName}";
        }

        public async Task<bool> DeleteAppointmentAsync(int id)
        {
            var appointment = await _uow.Appointments.GetByIdAsync(id);
            if (appointment == null) return false;

            if (!string.IsNullOrEmpty(appointment.Image))
            {
                DeleteImage(appointment.Image);
            }

            _uow.Appointments.Delete(appointment);
            await _uow.SaveChangesAsync();
            return true;
        }


        private void DeleteImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return;
            var fullPath = Path.Combine(_hosting.WebRootPath, imagePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
