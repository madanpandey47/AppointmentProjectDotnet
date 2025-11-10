using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly backend.Data.IUnitOfWork _uow;

        public CategoryService(backend.Data.IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _uow.Categories.GetAllAsync();
            return categories.Select(c => new CategoryDTO { Id = c.Id, Name = c.Name });
        }

        public async Task<CategoryDTO?> GetCategoryByIdAsync(int id)
        {
            var category = await _uow.Categories.GetByIdAsync(id);
            if (category == null) return null;
            return new CategoryDTO { Id = category.Id, Name = category.Name };
        }

        public async Task<CategoryDTO> CreateCategoryAsync(SaveCategoryDTO categoryDto)
        {
            var category = new Category { Name = categoryDto.Name };
            await _uow.Categories.AddAsync(category);
            await _uow.SaveChangesAsync();
            return new CategoryDTO { Id = category.Id, Name = category.Name };
        }

        public async Task<bool> UpdateCategoryAsync(int id, SaveCategoryDTO categoryDto)
        {
            var category = await _uow.Categories.GetByIdAsync(id);
            if (category == null) return false;

            category.Name = categoryDto.Name;
            _uow.Categories.Update(category);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _uow.Categories.GetByIdAsync(id);
            if (category == null) return false;

            _uow.Categories.Delete(category);
            await _uow.SaveChangesAsync();
            return true;
        }
    }
}
