namespace MyMoods.Domain.DTO
{
    public class MoodCounterDTO
    {
        public MoodCounterDTO(MoodType type, int count)
        {
            Mood = type;
            Count = count;
        }

        public MoodType Mood { get; private set; }
        public int Count { get; set; }
    }
}
