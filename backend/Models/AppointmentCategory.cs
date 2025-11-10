namespace backend.Models
{
    public class AppointmentCategory
    {
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
