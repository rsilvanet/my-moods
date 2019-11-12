using MyMoods.Shared.Domain.Enums;

namespace MyMoods.Shared.Domain.DTO
{
    public class TagDTO
    {
        public TagDTO(Tagg tag)
        {
            Id = tag.Id.ToString();
            Title = tag.Title;
            Type = tag.Type;
            Active = tag.Active;
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public TagType Type { get; set; }
        public bool Active { get; set; }
    }
}
