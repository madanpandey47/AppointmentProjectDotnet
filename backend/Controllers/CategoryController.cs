using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public CategoryController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<backend.DTOs.CategoryDTO>>> GetCategories()
        {
            var categories = await _uow.Categories.GetAllAsync();
            var categoryDtos = categories.Select(c => new backend.DTOs.CategoryDTO
            {
                Id = c.Id,
                Name = c.Name
            });
            return Ok(categoryDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<backend.DTOs.CategoryDTO>> GetCategory(int id)
        {
            var category = await _uow.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var categoryDto = new backend.DTOs.CategoryDTO
            {
                Id = category.Id,
                Name = category.Name
            };
            return Ok(categoryDto);
        }

        [HttpPost]
        public async Task<ActionResult<backend.DTOs.CategoryDTO>> CreateCategory([FromBody] backend.DTOs.SaveCategoryDTO categoryDto)
        {
            var category = new Category { Name = categoryDto.Name };
            await _uow.Categories.AddAsync(category);
            
            var resultDto = new backend.DTOs.CategoryDTO { Id = category.Id, Name = category.Name };
            return CreatedAtAction(nameof(GetCategory), new { id = resultDto.Id }, resultDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] backend.DTOs.SaveCategoryDTO categoryDto)
        {
            var category = await _uow.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            category.Name = categoryDto.Name;
            _uow.Categories.Update(category);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _uow.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            _uow.Categories.Delete(category);
            return NoContent();
        }
    }
}
