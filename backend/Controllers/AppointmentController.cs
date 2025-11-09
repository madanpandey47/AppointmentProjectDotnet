
﻿using backend.Models;
﻿using backend.Repositories;
﻿using Microsoft.AspNetCore.Mvc;
﻿
﻿namespace backend.Controllers
﻿{
﻿    [ApiController]
﻿    [Route("[controller]")]
﻿    public class AppointmentController : ControllerBase
﻿    {
﻿        private readonly IAppointmentRepository _repo;
﻿        private readonly IWebHostEnvironment _hosting;
﻿
﻿        public AppointmentController(IAppointmentRepository repo, IWebHostEnvironment hosting)
﻿        {
﻿            _repo = repo;
﻿            _hosting = hosting;
﻿        }
﻿
﻿        // Get all
﻿                [HttpGet]
﻿                public async Task<IActionResult> GetAll()
﻿                {
﻿                    var data = await _repo.GetAllWithCategoryAsync();
﻿                    var dtos = data.Select(a => new backend.DTOs.AppointmentDTO
﻿                    {
﻿                        Id = a.Id,
﻿                        Title = a.Title,
﻿                        Description = a.Description,
﻿                        Date = a.Date,
﻿                        Image = a.Image,
﻿                        CategoryId = a.CategoryId,
﻿                        CategoryName = a.Category?.Name ?? "N/A"
﻿                    });
﻿                    return Ok(dtos);
﻿                }
﻿        
﻿                // Get by id
﻿                [HttpGet("{id}")]
﻿                public async Task<IActionResult> Get(int id)
﻿                {
﻿                    var appointment = await _repo.GetByIdWithCategoryAsync(id);
﻿                    if (appointment == null)
﻿                        return NotFound();
﻿        
﻿                    var dto = new backend.DTOs.AppointmentDTO
﻿                    {
﻿                        Id = appointment.Id,
﻿                        Title = appointment.Title,
﻿                        Description = appointment.Description,
﻿                        Date = appointment.Date,
﻿                        Image = appointment.Image,
﻿                        CategoryId = appointment.CategoryId,
﻿                        CategoryName = appointment.Category?.Name ?? "N/A"
﻿                    };
﻿        
﻿                    return Ok(dto);
﻿                }﻿
﻿        // Create

﻿                [HttpPost]
﻿                public async Task<IActionResult> Create([FromForm] backend.DTOs.CreateAppointmentDTO appointmentDto, IFormFile? imageFile)
﻿                {
﻿                    var appointment = new Appointment
﻿                    {
﻿                        Title = appointmentDto.Title,
﻿                        Description = appointmentDto.Description,
﻿                        Date = appointmentDto.Date,
﻿                        CategoryId = appointmentDto.CategoryId
﻿                    };
﻿        
﻿                    if (imageFile != null)
﻿                    {
﻿                        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
﻿                        var filePath = Path.Combine(_hosting.WebRootPath, "uploads", fileName);
﻿                        using (var stream = new FileStream(filePath, FileMode.Create))
﻿                        {
﻿                            await imageFile.CopyToAsync(stream);
﻿                        }
﻿                        appointment.Image = $"/uploads/{fileName}";
﻿                    }
﻿        
﻿                    await _repo.AddAsync(appointment);
﻿                    return CreatedAtAction(nameof(Get), new { id = appointment.Id }, appointment);
﻿                }﻿
﻿        ﻿        ﻿        ﻿        //  Update
﻿
﻿        ﻿        ﻿        ﻿                [HttpPut("{id}")]
﻿        ﻿        ﻿        ﻿                public async Task<IActionResult> Update(int id, [FromForm] backend.DTOs.CreateAppointmentDTO appointmentDto, IFormFile? imageFile)
﻿        ﻿        ﻿        ﻿                {
﻿        ﻿        ﻿        ﻿                    var existing = await _repo.GetByIdAsync(id);
﻿        ﻿        ﻿        ﻿                    if (existing == null)
﻿        ﻿        ﻿        ﻿                        return NotFound();
﻿        ﻿        ﻿        ﻿        
﻿        ﻿        ﻿        ﻿                    // Map properties from the DTO to the existing entity
﻿        ﻿        ﻿        ﻿                    existing.Title = appointmentDto.Title;
﻿        ﻿        ﻿        ﻿                    existing.Description = appointmentDto.Description;
﻿        ﻿        ﻿        ﻿                    existing.Date = appointmentDto.Date;
﻿        ﻿        ﻿        ﻿                    existing.CategoryId = appointmentDto.CategoryId;
﻿        ﻿        ﻿        ﻿        
﻿        ﻿        ﻿        ﻿                    if (imageFile != null)
﻿        ﻿        ﻿        ﻿                    {
﻿        ﻿        ﻿        ﻿                        // Delete old image
﻿        ﻿        ﻿        ﻿                        if (!string.IsNullOrEmpty(existing.Image))
﻿        ﻿        ﻿        ﻿                        {
﻿        ﻿        ﻿        ﻿                            var oldImagePath = Path.Combine(_hosting.WebRootPath, existing.Image.TrimStart('/'));
﻿        ﻿        ﻿        ﻿                            if (System.IO.File.Exists(oldImagePath))
﻿        ﻿        ﻿        ﻿                            {
﻿        ﻿        ﻿        ﻿                                System.IO.File.Delete(oldImagePath);
﻿        ﻿        ﻿        ﻿                            }
﻿        ﻿        ﻿        ﻿                        }
﻿        ﻿        ﻿        ﻿        
﻿        ﻿        ﻿        ﻿                        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
﻿        ﻿        ﻿        ﻿                        var filePath = Path.Combine(_hosting.WebRootPath, "uploads", fileName);
﻿        ﻿        ﻿        ﻿                        using (var stream = new FileStream(filePath, FileMode.Create))
﻿        ﻿        ﻿        ﻿                        {
﻿        ﻿        ﻿        ﻿                            await imageFile.CopyToAsync(stream);
﻿        ﻿        ﻿        ﻿                        }
﻿        ﻿        ﻿        ﻿                        existing.Image = $"/uploads/{fileName}";
﻿        ﻿        ﻿        ﻿                    }
﻿        ﻿        ﻿        ﻿        
﻿        ﻿        ﻿        ﻿                    _repo.Update(existing);
﻿        ﻿        ﻿        ﻿                    return NoContent();
﻿        ﻿        ﻿        ﻿                }        //  Delete
﻿        [HttpDelete("{id}")]
﻿        public async Task<IActionResult> Delete(int id)
﻿        {
﻿            var appointment = await _repo.GetByIdAsync(id);
﻿            if (appointment == null)
﻿                return NotFound();
﻿
﻿            // Delete image if it exists
﻿            if (!string.IsNullOrEmpty(appointment.Image))
﻿            {
﻿                var imagePath = Path.Combine(_hosting.WebRootPath, appointment.Image.TrimStart('/'));
﻿                if (System.IO.File.Exists(imagePath))
﻿                {
﻿                    System.IO.File.Delete(imagePath);
﻿                }
﻿            }
﻿
﻿            _repo.Delete(appointment);
﻿
﻿            return NoContent();
﻿        }
﻿    }
﻿}
﻿
