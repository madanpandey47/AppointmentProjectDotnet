using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

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
        public List<int> CategoryIds { get; set; } = new List<int>();
    }
}