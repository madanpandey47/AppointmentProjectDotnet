using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class CreateAppointmentDTO
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
