using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace backend.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<AppointmentCategory> AppointmentCategories { get; set; } = new List<AppointmentCategory>();
    }
}