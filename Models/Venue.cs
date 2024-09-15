namespace Bogoda.Models
{
    public class Venue(string name, string address, int categoryId, string? description)
    {
        public int Id { get; set; }
        public string Name { get; set; } = name;
        public string Address { get; set; } = address;
        public string? Description { get; set; } = description;
        public int CategoryId { get; set; } = categoryId;
        public virtual Category? Category { get; set; }
        public virtual List<Tag> Tags { get; set; } = [];
    }
    public class VenueDTO(Venue venue)
    {
        public int Id { get; set; } = venue.Id;
        public string Name { get; set; } = venue.Name;
        public string Address { get; set; } = venue.Address;
        public string? Description { get; set; } = venue.Description;
        public CategoryDTO? Category { get; set; } = new CategoryDTO(venue.Category);
        public List<TagDTO> Tags { get; set; } = venue.Tags.Select(x => new TagDTO(x)).ToList();
    }
}
