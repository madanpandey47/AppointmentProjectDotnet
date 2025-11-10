using backend.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO?> GetCategoryByIdAsync(int id);
        Task<CategoryDTO> CreateCategoryAsync(SaveCategoryDTO categoryDto);
        Task<bool> UpdateCategoryAsync(int id, SaveCategoryDTO categoryDto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
