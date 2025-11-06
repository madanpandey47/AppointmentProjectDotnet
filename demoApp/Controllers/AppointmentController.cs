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

        // ✅ GET ALL
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var appointments = await _context.Appointments
            .Select(a => new
            {
                a.Id,
                a.Title,
                a.Description,
                a.Date,
                Image = string.IsNullOrEmpty(a.Image)
                    ? null
                    : $"{baseUrl}/uploads/{a.Image}"
            })
            .ToListAsync();

            return Ok(appointments);
        }

        // ✅ ADD
        [HttpPost]
        public async Task<IActionResult> Add(
            [FromForm] IFormFile? image,
            [FromForm] string title,
            [FromForm] string description,
            [FromForm] DateTime date
        )
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

            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            return Ok(new
            {
                appointment.Id,
                appointment.Title,
                appointment.Description,
                appointment.Date,
                Image = fileName != null ? $"{baseUrl}/uploads/{fileName}" : null
            });
        }

        // ✅ UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromForm] IFormFile? image,
            [FromForm] string title,
            [FromForm] string description,
            [FromForm] DateTime date
        )
        {
            var appt = await _context.Appointments.FindAsync(id);
            if (appt == null) return NotFound();

            appt.Title = title;
            appt.Description = description;
            appt.Date = date;

            if (image != null && image.Length > 0)
            {
                if (!string.IsNullOrEmpty(appt.Image))
                {
                    string oldPath = Path.Combine(_env.WebRootPath, "uploads", appt.Image);
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }

                string uploadPath = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                string fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                string filePath = Path.Combine(uploadPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await image.CopyToAsync(stream);

                appt.Image = fileName;
            }

            await _context.SaveChangesAsync();

            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            return Ok(new
            {
                appt.Id,
                appt.Title,
                appt.Description,
                appt.Date,
                Image = appt.Image != null ? $"{baseUrl}/uploads/{appt.Image}" : null
            });
        }

        // ✅ DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

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
