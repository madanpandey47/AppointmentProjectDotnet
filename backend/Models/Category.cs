namespace backend.Models
{
    public class Category
    {
        public int cId { get; set; }
        public string cname { get; set; } = null!;
        public ICollection<Appointment>? Appointments { get; set; }
    }
}
