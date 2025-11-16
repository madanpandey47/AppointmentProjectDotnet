using System.Collections.Generic;

namespace backend.DTOs
{
    public class PaginatedAppointmentsDTO
    {
        public IEnumerable<AppointmentDTO> Appointments { get; set; }
        public int TotalCount { get; set; }
    }
}
