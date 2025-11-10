using backend.DTOs;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _service;

        public AppointmentController(IAppointmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dtos = await _service.GetAllAppointmentsAsync();
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var dto = await _service.GetAppointmentByIdAsync(id);
            if (dto == null)
                return NotFound();
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateAppointmentDTO appointmentDto, IFormFile? imageFile)
        {
            var createdAppointment = await _service.CreateAppointmentAsync(appointmentDto, imageFile);
            return CreatedAtAction(nameof(Get), new { id = createdAppointment.Id }, createdAppointment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] CreateAppointmentDTO appointmentDto, IFormFile? imageFile)
        {
            var result = await _service.UpdateAppointmentAsync(id, appointmentDto, imageFile);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAppointmentAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}