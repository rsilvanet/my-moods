using System;

namespace MyMoods.Domain.DTO
{
    public class DailySimpleDTO
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public TopDTO Top { get; set; }
        public AvgDTO Avg { get; set; }

        public class TopDTO
        {
            public MoodType Mood { get; set; }
            public string Image { get; set; }
            public int Count { get; set; }
        }

        public class AvgDTO
        {
            public MoodType Mood { get; set; }
            public string Image { get; set; }
            public double Points { get; set; }
        }
    }
}
