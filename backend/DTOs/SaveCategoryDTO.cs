using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class SaveCategoryDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
