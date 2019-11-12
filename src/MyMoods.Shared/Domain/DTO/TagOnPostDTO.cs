using MyMoods.Shared.Domain.Enums;

namespace MyMoods.Shared.Domain.DTO
{
    public class TagOnPostDTO
    {
        public string Title { get; set; }
        public TagType? Type { get; set; }
    }
}
