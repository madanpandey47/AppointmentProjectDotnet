
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
﻿        [HttpGet]
﻿        public async Task<IActionResult> GetAll()
﻿        {
﻿            var data = await _repo.GetAllAsync();
﻿            return Ok(data);
﻿        }
﻿
﻿        // Get by id
﻿        [HttpGet("{id}")]
﻿        public async Task<IActionResult> Get(int id)
﻿        {
﻿            var appointment = await _repo.GetByIdAsync(id);
﻿            if (appointment == null)
﻿                return NotFound();
﻿
﻿            return Ok(appointment);
﻿        }
﻿
﻿        // Create
﻿        [HttpPost]
﻿        public async Task<IActionResult> Create([FromForm] Appointment appointment, IFormFile? imageFile)
﻿        {
﻿            if (imageFile != null)
﻿            {
﻿                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
﻿                var filePath = Path.Combine(_hosting.WebRootPath, "uploads", fileName);
﻿                using (var stream = new FileStream(filePath, FileMode.Create))
﻿                {
﻿                    await imageFile.CopyToAsync(stream);
﻿                }
﻿                appointment.Image = $"/uploads/{fileName}";
﻿            }
﻿
﻿            await _repo.AddAsync(appointment);
﻿            return CreatedAtAction(nameof(Get), new { id = appointment.Id }, appointment);
﻿        }
﻿
﻿        ﻿        ﻿        ﻿        //  Update
﻿
﻿        ﻿        ﻿        ﻿        [HttpPut("{id}")]
﻿
﻿        ﻿        ﻿        ﻿        public async Task<IActionResult> Update(int id, [FromForm] Appointment appointment, IFormFile? imageFile)
﻿
﻿        ﻿        ﻿        ﻿        {
﻿
﻿        ﻿        ﻿        ﻿            var existing = await _repo.GetByIdAsync(id);
﻿
﻿        ﻿        ﻿        ﻿            if (existing == null)
﻿
﻿        ﻿        ﻿        ﻿                return NotFound();
﻿
﻿        ﻿        ﻿        
﻿
﻿        ﻿        ﻿        ﻿            if (imageFile != null)
﻿
﻿        ﻿        ﻿        ﻿            {
﻿
﻿        ﻿        ﻿        ﻿                // Delete old image if it exists
﻿
﻿        ﻿        ﻿        ﻿                if (!string.IsNullOrEmpty(existing.Image))
﻿
﻿        ﻿        ﻿        ﻿                {
﻿
﻿        ﻿        ﻿        ﻿                    var oldImagePath = Path.Combine(_hosting.WebRootPath, existing.Image.TrimStart('/'));
﻿
﻿        ﻿        ﻿        ﻿                    if (System.IO.File.Exists(oldImagePath))
﻿
﻿        ﻿        ﻿        ﻿                    {
﻿
﻿        ﻿        ﻿        ﻿                        System.IO.File.Delete(oldImagePath);
﻿
﻿        ﻿        ﻿        ﻿                    }
﻿
﻿        ﻿        ﻿        ﻿                }
﻿
﻿        ﻿        ﻿        
﻿
﻿        ﻿        ﻿        ﻿                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
﻿
﻿        ﻿        ﻿        ﻿                var filePath = Path.Combine(_hosting.WebRootPath, "uploads", fileName);
﻿
﻿        ﻿        ﻿        ﻿                using (var stream = new FileStream(filePath, FileMode.Create))
﻿
﻿        ﻿        ﻿        ﻿                {
﻿
﻿        ﻿        ﻿        ﻿                    await imageFile.CopyToAsync(stream);
﻿
﻿        ﻿        ﻿        ﻿                }
﻿
﻿        ﻿        ﻿        ﻿                existing.Image = $"/uploads/{fileName}";
﻿
﻿        ﻿        ﻿        ﻿            }
﻿
﻿        ﻿        ﻿        
﻿
﻿        ﻿        ﻿        ﻿            existing.Title = appointment.Title;
﻿
﻿        ﻿        ﻿        ﻿            existing.Description = appointment.Description;
﻿
﻿        ﻿        ﻿        ﻿            existing.Date = appointment.Date;
﻿
﻿        ﻿        ﻿        ﻿
﻿
﻿        ﻿        ﻿        ﻿            _repo.Update(existing);
﻿
﻿        ﻿        ﻿        ﻿            return NoContent();
﻿
﻿        ﻿        ﻿        ﻿        }﻿        //  Delete
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
