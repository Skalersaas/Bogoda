using Bogoda.Data;
using Bogoda.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bogoda.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController(ApplicationContext context) : ControllerBase
    {
        /// <summary>
        /// Создает новую категорию с переданными параметрами.
        /// </summary>
        /// <param name="name">Имя категории.</param>
        /// <param name="description">Описание категории (необязательно).</param>
        /// <returns>Возвращает данные по созданной категории.</returns>
        [HttpPost("create")]
        public ObjectResult Create(string name, string? description)
        {
            Category category = new(name, description);
            context.Categories.Add(category);
            context.SaveChanges();

            return Ok(new CategoryDTO(category));
        }

        /// <summary>
        /// Получает список всех категорий.
        /// </summary>
        /// <returns>Возвращает список всех объектов CategoryDTO в формате JSON.</returns>
        [HttpGet("all")]
        public JsonResult GetAll()
        {
            return new(context.Categories
                .Select(c => new CategoryDTO(c)));
        }

        /// <summary>
        /// Возвращает список категорий, содержащих в названии указанную строку.
        /// </summary>
        /// <param name="name">Часть названия категории для поиска.</param>
        /// <returns>Возвращает список объектов CategoryDTO, соответствующих критерию поиска.</returns>
        [HttpGet("name/{name}")]
        public JsonResult GetByName(string name)
        {
            return new(context.Categories
                .Where(c => c.Name.ToLower().Contains(name.ToLower()))
                .Select(c => new CategoryDTO(c))
            );
        }

        /// <summary>
        /// Возвращает категорию по её идентификатору.
        /// </summary>
        /// <param name="Id">Идентификатор категории.</param>
        /// <returns>Возвращает объект CategoryDTO по идентификатору или ошибку NotFound, если категория не найдена.</returns>
        [HttpGet("id/{Id}")]
        public JsonResult GetById(int Id)
        {
            return new(context.Categories
                .Where(c => c.Id == Id)
                .Select(c => new CategoryDTO(c))
            );
        }

        /// <summary>
        /// Обновляет информацию о категории.
        /// </summary>
        /// <param name="Id">Идентификатор категории.</param>
        /// <param name="name">Новое имя категории.</param>
        /// <param name="description">Новое описание категории (необязательно).</param>
        /// <returns>Возвращает обновленный объект CategoryDTO или ошибку NotFound, если категория не найдена.</returns>
        [HttpPut("update")]
        public ObjectResult Update(int Id, string name, string? description)
        {
            Category? category = context.Categories.FirstOrDefault(c => c.Id == Id);
            
            if (category == null)
                return NotFound(new { response = "Category not found" });

            category.Name = name;
            category.Description = description;
            context.SaveChanges();
            return Ok(new CategoryDTO(category));
        }

        /// <summary>
        /// Удаляет категорию по идентификатору.
        /// </summary>
        /// <param name="Id">Идентификатор категории.</param>
        /// <returns>Возвращает успешный результат или ошибку NotFound, если категория не найдена.</returns>
        [HttpDelete("delete")]
        public ObjectResult Delete(int Id)
        {
            Category? category = context.Categories.FirstOrDefault(c => c.Id == Id);

            if (category == null)
                return NotFound(new { response = "Category not found" });

            context.Categories.Remove(category);
            context.SaveChanges();

            return Ok("Category was deleted");
        }
    }
}
