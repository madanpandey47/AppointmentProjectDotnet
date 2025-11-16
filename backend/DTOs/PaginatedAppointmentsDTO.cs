using System.Collections.Generic;

namespace backend.DTOs
{
    public class PaginatedAppointmentsDTO
    {
        public IEnumerable<AppointmentDTO> Appointments { get; set; } = new List<AppointmentDTO>();
        public int TotalCount { get; set; }
    }
}
