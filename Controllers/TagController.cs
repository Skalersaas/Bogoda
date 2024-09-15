using Bogoda.Data;
using Bogoda.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bogoda.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagController(ApplicationContext context) : ControllerBase
    {
        /// <summary>
        /// Создает новый тег с переданными параметрами.
        /// </summary>
        /// <param name="name">Имя тега.</param>
        /// <param name="description">Описание тега (необязательно).</param>
        /// <returns>Возвращает данные по созданному тегу.</returns>
        [HttpPost("create")]
        public ObjectResult Create(string name, string? description)
        {
            Tag tag = new(name, description);
            context.Tags.Add(tag);
            context.SaveChanges();

            return Ok(new TagDTO(tag));
        }

        /// <summary>
        /// Получает список всех тегов.
        /// </summary>
        /// <returns>Возвращает список всех объектов TagDTO в формате JSON.</returns>
        [HttpGet("all")]
        public JsonResult GetAll()
        {
            return new(context.Tags
                .Select(t => new TagDTO(t)));
        }

        /// <summary>
        /// Возвращает список тегов, содержащих в названии указанную строку.
        /// </summary>
        /// <param name="name">Часть названия тега для поиска.</param>
        /// <returns>Возвращает список объектов TagDTO, соответствующих критерию поиска.</returns>
        [HttpGet("name/{name}")]
        public JsonResult GetByName(string name)
        {
            return new(context.Tags
                .Where(t => t.Name.ToLower().Contains(name.ToLower()))
                .Select(t => new TagDTO(t))
            );
        }

        /// <summary>
        /// Возвращает тег по его идентификатору.
        /// </summary>
        /// <param name="Id">Идентификатор тега.</param>
        /// <returns>Возвращает объект TagDTO по идентификатору или ошибку NotFound, если тег не найден.</returns>
        [HttpGet("id/{Id}")]
        public JsonResult GetById(int Id)
        {
            return new(context.Tags
                .Where(t => t.Id == Id)
                .Select(t => new TagDTO(t))
            );
        }

        /// <summary>
        /// Обновляет информацию о теге.
        /// </summary>
        /// <param name="Id">Идентификатор тега.</param>
        /// <param name="name">Новое имя тега.</param>
        /// <param name="description">Новое описание тега (необязательно).</param>
        /// <returns>Возвращает обновленный объект TagDTO или ошибку NotFound, если тег не найден.</returns>
        [HttpPut("update")]
        public ObjectResult Update(int Id, string name, string? description)
        {
            Tag? tag = context.Tags.FirstOrDefault(t => t.Id == Id);

            if (tag == null)
                return NotFound(new { response = "Tag not found" });

            tag.Name = name;
            tag.Description = description;
            context.SaveChanges();
            return Ok(new TagDTO(tag));
        }

        /// <summary>
        /// Удаляет тег по идентификатору.
        /// </summary>
        /// <param name="Id">Идентификатор тега.</param>
        /// <returns>Возвращает успешный результат или ошибку NotFound, если тег не найден.</returns>
        [HttpDelete("delete")]
        public ObjectResult Delete(int Id)
        {
            Tag? tag = context.Tags.FirstOrDefault(t => t.Id == Id);

            if (tag == null)
                return NotFound(new { response = "Tag not found" });

            context.Tags.Remove(tag);
            context.SaveChanges();

            return Ok("Tag was deleted");
        }
    }
}
