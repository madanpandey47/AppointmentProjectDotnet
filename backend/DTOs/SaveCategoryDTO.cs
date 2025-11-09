using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class SaveCategoryDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
