using System;

namespace MyMoods.Shared.Domain.DTO
{
    public class ReviewsResumeDTO
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public TopDTO Top { get; set; }
        public AvgDTO Avg { get; set; }

        public class TopDTO
        {
            public MoodType Mood { get; set; }
            public int Count { get; set; }
        }

        public class AvgDTO
        {
            public MoodType Mood { get; set; }
            public double Points { get; set; }
        }
    }
}
