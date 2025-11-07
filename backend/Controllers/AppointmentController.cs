using backend.Models;
using backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _repo;

        public AppointmentController(IAppointmentRepository repo)
        {
            _repo = repo;
        }

        // ✅ Get all
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _repo.GetAllAsync();
            return Ok(data);
        }

        // ✅ Get by id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var appointment = await _repo.GetByIdWithUserAsync(id);
            if (appointment == null)
                return NotFound();

            // Only return minimal user info if exists
            var result = new
            {
                appointment.Id,
                appointment.Title,
                appointment.Description,
                appointment.Date,
                appointment.Image,
                User = appointment.User == null ? null : new
                {
                    appointment.User.Id,
                    appointment.User.Username
                }
            };

            return Ok(result);
        }

        // ✅ Create
        [HttpPost]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            await _repo.AddAsync(appointment);
            return CreatedAtAction(nameof(Get), new { id = appointment.Id }, appointment);
        }

        // ✅ Update
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Appointment appointment)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.Title = appointment.Title;
            existing.Description = appointment.Description;
            existing.Date = appointment.Date;

            _repo.Update(existing);
            return NoContent();
        }

        // ✅ Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _repo.GetByIdAsync(id);
            if (appointment == null)
                return NotFound();

            _repo.Delete(appointment);

            return NoContent();
        }
    }
}
