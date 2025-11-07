namespace backend.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string? Image { get; set; }
        public int cId { get; set; }
        public Category? Category { get; set; }
    }
}
