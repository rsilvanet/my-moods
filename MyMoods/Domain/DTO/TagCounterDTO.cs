namespace MyMoods.Domain.DTO
{
    public class TagCounterDTO : TagDTO
    {
        public TagCounterDTO(Tagg tag, long count) : base(tag)
        {
            Count = count;
        }

        public long Count { get; set; }
    }
}
