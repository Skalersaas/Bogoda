using Bogoda.Data;
using Bogoda.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bogoda.Controllers
{
    [ApiController]
    [Route("[controller]")]
    // Контроллер для работы с таблицей Venue
    public class VenueController(ApplicationContext context) : ControllerBase
    {
         /// <summary>
         /// Создает объект Venue с переданными параметрами. Также проверяет наличие выбранной категории.
         /// </summary>
         /// <param name="name">Имя заведения.</param>
         /// <param name="address">Адрес заведения.</param>
         /// <param name="categoryId">Идентификатор категории заведения.</param>
         /// <param name="description">Описание заведения (необязательно).</param>
         /// <returns>Возвращает данные по созданному объекту или ошибку NotFound, если категория не найдена.</returns>
        [HttpPost("create")]
        public ObjectResult Create(string name, string address, int categoryId, string? description)
        {
            Category? category = context.Categories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null)
                return NotFound(new { response = "Category not found" });

            Venue venue = new(name, address, categoryId, description);
            context.Venues.Add(venue);
            context.SaveChanges();

            return Ok(new VenueDTO(venue));
        }

        /// <summary>
        /// Получает список всех заведений с их категориями и тегами.
        /// </summary>
        /// <returns>Возвращает список всех объектов VenueDTO в формате JSON.</returns>
        [HttpGet("all")]
        public JsonResult GetAll()
        {
            return new(context.Venues
                .Include(v => v.Category)
                .Include(v => v.Tags)
                .Select(v => new VenueDTO(v)));
        }

        /// <summary>
        /// Возвращает список заведений, содержащих в названии указанную строку.
        /// </summary>
        /// <param name="name">Часть названия заведения для поиска.</param>
        /// <returns>Возвращает список объектов VenueDTO, соответствующих критерию поиска.</returns>
        [HttpGet("name/{name}")]
        public JsonResult GetByName(string name)
        {
            return new(context.Venues
                .Where(v => v.Name.ToLower().Contains(name.ToLower()))
                .Include(v => v.Category)
                .Include(v => v.Tags)
                .Select(v => new VenueDTO(v)));
        }

        /// <summary>
        /// Возвращает заведение по его идентификатору.
        /// </summary>
        /// <param name="Id">Идентификатор заведения.</param>
        /// <returns>Возвращает объект VenueDTO по идентификатору или ошибку NotFound, если заведение не найдено.</returns>
        [HttpGet("id/{Id}")]
        public JsonResult GetById(int Id)
        {
            return new(
                context.Venues
                .Where(v => v.Id == Id)
                .Include(v => v.Category)
                .Include(v => v.Tags)
                .Select(v => new VenueDTO(v))
            );
        }

        /// <summary>
        /// Получает список заведений по тегу.
        /// </summary>
        /// <param name="tagId">Идентификатор тега.</param>
        /// <returns>Возвращает список объектов VenueDTO, соответствующих тегу, или ошибку NotFound, если тег не найден.</returns>
        [HttpGet("tag/{tagId}")]
        public ObjectResult GetByTag(int tagId)
        {
            Tag? tag = context.Tags.FirstOrDefault(t => t.Id == tagId);
            if (tag == null)
                return NotFound(new { response = "There's no tag with this tagId" });

            return new(
                context.Venues
                .Where(v => v.Tags.Contains(tag))
                .Include(v => v.Tags)
                .Include(v => v.Category)
                .Select(v => new VenueDTO(v))
            );
        }

        /// <summary>
        /// Получает список заведений по категории.
        /// </summary>
        /// <param name="categoryId">Идентификатор категории.</param>
        /// <returns>Возвращает список объектов VenueDTO, соответствующих категории, или ошибку NotFound, если категория не найдена.</returns>
        [HttpGet("category/{categoryId}")]
        public ObjectResult GetByCategory(int categoryId)
        {
            Category? category = context.Categories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null)
                return NotFound(new { response = "There's no category with this categoryId" });

            return new(
                context.Venues
                .Where(v => v.Category == category)
                .Include(v => v.Tags)
                .Include(v => v.Category)
                .Select(v => new VenueDTO(v))
            );
        }

        /// <summary>
        /// Обновляет информацию о заведении.
        /// </summary>
        /// <param name="Id">Идентификатор заведения.</param>
        /// <param name="name">Новое имя заведения.</param>
        /// <param name="address">Новый адрес заведения.</param>
        /// <param name="categoryId">Новая категория заведения.</param>
        /// <param name="description">Новое описание заведения (необязательно).</param>
        /// <returns>Возвращает обновленный объект VenueDTO или ошибку NotFound, если заведение не найдено.</returns>
        [HttpPut("update")]
        public ObjectResult Update(int Id, string name, string address, int categoryId, string? description)
        {
            Venue? venue = context.Venues.FirstOrDefault(v => v.Id == Id);

            if (venue == null)
                return NotFound(new { response = "Venue not found" });

            venue.Name = name;
            venue.Address = address;
            venue.CategoryId = categoryId;
            venue.Description = description;
            context.SaveChanges();

            return Ok(new VenueDTO(venue));
        }

        /// <summary>
        /// Добавляет тег к заведению.
        /// </summary>
        /// <param name="venueId">Идентификатор заведения.</param>
        /// <param name="tagId">Идентификатор тега.</param>
        /// <returns>Возвращает обновленный объект VenueDTO или сообщение об ошибке, если тег или заведение не найдены.</returns>
        [HttpPatch("addtag")]
        public ObjectResult AddTag(int venueId, int tagId)
        {
            Venue? venue = context.Venues.Include(v => v.Tags).FirstOrDefault(v => v.Id == venueId);
            if (venue == null)
                return NotFound(new { response = "Venue not found" });

            Tag? tag = context.Tags.FirstOrDefault(t => t.Id == tagId);
            if (tag == null)
                return NotFound(new { response = "Tag not found" });

            if (venue.Tags.Contains(tag))
                return Conflict(new { response = "Tag already exists" });

            venue.Tags.Add(tag);
            context.SaveChanges();

            return Ok(new VenueDTO(venue));
        }

        /// <summary>
        /// Удаляет тег из заведения.
        /// </summary>
        /// <param name="venueId">Идентификатор заведения.</param>
        /// <param name="tagId">Идентификатор тега.</param>
        /// <returns>Возвращает обновленный объект VenueDTO или сообщение об ошибке, если тег или заведение не найдены.</returns>
        [HttpPatch("removetag")]
        public ObjectResult RemoveTag(int venueId, int tagId)
        {
            Venue? venue = context.Venues.Include(v => v.Tags).FirstOrDefault(v => v.Id == venueId);
            if (venue == null)
                return NotFound(new { response = "Venue not found" });

            Tag? tag = context.Tags.FirstOrDefault(t => t.Id == tagId);
            if (tag == null)
                return NotFound(new { response = "Tag not found" });

            if (!venue.Tags.Contains(tag))
                return Conflict(new { response = "This tag doesn't exist on this venue" });

            venue.Tags.Remove(tag);
            context.SaveChanges();

            return Ok(new VenueDTO(venue));
        }

        /// <summary>
        /// Удаляет заведение по идентификатору.
        /// </summary>
        /// <param name="Id">Идентификатор заведения.</param>
        /// <returns>Возвращает успешный результат или ошибку NotFound, если заведение не найдено.</returns>
        [HttpDelete("delete")]
        public ObjectResult Delete(int Id)
        {
            Venue? venue = context.Venues.FirstOrDefault(v => v.Id == Id);

            if (venue == null)
                return NotFound(new { response = "Venue not found" });

            context.Venues.Remove(venue);
            context.SaveChanges();

            return Ok("Venue was deleted");
        }

    }
}
