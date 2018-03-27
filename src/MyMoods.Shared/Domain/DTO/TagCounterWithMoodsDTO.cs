using System.Collections.Generic;

namespace MyMoods.Shared.Domain.DTO
{
    public class TagCounterWithMoodsDTO : TagCounterDTO
    {
        public TagCounterWithMoodsDTO(Tagg tag, long count, IList<MoodCounterDTO> moods) : base(tag, count)
        {
            Moods = moods;
        }

        public IList<MoodCounterDTO> Moods { get; set; }
    }
}
