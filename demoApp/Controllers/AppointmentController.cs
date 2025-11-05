using demoApp.Data;
using demoApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demoApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AppointmentController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/Appointment
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var appointments = await _context.Appointments.ToListAsync();

            // Prepend image path to serve images
            var result = appointments.Select(a => new
            {
                a.Id,
                a.Title,
                a.Description,
                a.Date,
                Image = a.Image != null ? $"{Request.Scheme}://{Request.Host}/uploads/{a.Image}" : null
            });

            return Ok(result);
        }

        // POST: api/Appointment
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] IFormFile? image, [FromForm] string title, [FromForm] string description, [FromForm] DateTime date)
        {
            string? fileName = null;

            if (image != null && image.Length > 0)
            {
                string uploadPath = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                string filePath = Path.Combine(uploadPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await image.CopyToAsync(stream);
            }

            var appointment = new Appointment
            {
                Title = title,
                Description = description,
                Date = date,
                Image = fileName
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            // Return appointment with full image path
            return Ok(new
            {
                appointment.Id,
                appointment.Title,
                appointment.Description,
                appointment.Date,
                Image = fileName != null ? $"{Request.Scheme}://{Request.Host}/uploads/{fileName}" : null
            });
        }

        // PUT: api/Appointment/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] IFormFile? image, [FromForm] string title, [FromForm] string description, [FromForm] DateTime date)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            appointment.Title = title;
            appointment.Description = description;
            appointment.Date = date;

            if (image != null && image.Length > 0)
            {
                // Delete old image if exists
                if (!string.IsNullOrEmpty(appointment.Image))
                {
                    string oldPath = Path.Combine(_env.WebRootPath, "uploads", appointment.Image);
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }

                string uploadPath = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                string fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                string filePath = Path.Combine(uploadPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await image.CopyToAsync(stream);

                appointment.Image = fileName;
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                appointment.Id,
                appointment.Title,
                appointment.Description,
                appointment.Date,
                Image = appointment.Image != null ? $"{Request.Scheme}://{Request.Host}/uploads/{appointment.Image}" : null
            });
        }

        // DELETE: api/Appointment/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            // Delete image file if exists
            if (!string.IsNullOrEmpty(appointment.Image))
            {
                string filePath = Path.Combine(_env.WebRootPath, "uploads", appointment.Image);
                if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
