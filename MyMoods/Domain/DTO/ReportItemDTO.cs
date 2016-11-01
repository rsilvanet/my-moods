using System;

namespace MyMoods.Domain.DTO
{
    public class ReportItemDTO
    {
        public ReportItemDTO()
        {
            Top = new CounterDTO();
            Avg = new CounterDTO();
        }

        public DateTime Date { get; set; }
        public int Count { get; set; }
        public double Points { get; set; }
        public CounterDTO Top { get; set; }
        public CounterDTO Avg { get; set; }

        public class CounterDTO
        {
            public MoodType Mood { get; set; }
            public string Image { get; set; }
            public int Count { get; set; }
        }
    }
}
