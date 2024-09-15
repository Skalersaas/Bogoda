namespace Bogoda.Models
{
    public class Category(string name, string? description)
    {
        public int Id { get; set; }
        public string Name { get; set; } = name;
        public string? Description { get; set; } = description;
        public virtual List<Venue> Venues { get; set; } = [];
    }
    public class CategoryDTO(Category category)
    {
        public int Id { get; set; } = category.Id;
        public string Name { get; set; } = category.Name;
        public string? Description { get; set; } = category.Description;
    }
}
