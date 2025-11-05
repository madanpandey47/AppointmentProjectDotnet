namespace demoApp.Models
{
public class Appointment
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string? Image { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
