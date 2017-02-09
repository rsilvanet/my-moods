namespace MyMoods.Domain.DTO
{
    public class MaslowCounterDTO
    {
        public MaslowCounterDTO(TagType type, long count)
        {
            Area = type;
            Count = count;
        }

        public TagType Area { get; private set; }
        public long Count { get; set; }
    }
}
