namespace Bogoda.Models
{
    public class Tag(string name, string? description)
    {
        public int Id { get; set; }
        public string Name { get; set; } = name;
        public string? Description { get; set; } = description;
        public virtual List<Venue> Venues { get; set; } = [];
    }
    public class TagDTO(Tag tag)
    {
        public int Id { get; set; } = tag.Id;
        public string Name { get; set; } = tag.Name;
        public string? Description { get; set; } = tag.Description;
    }
}
