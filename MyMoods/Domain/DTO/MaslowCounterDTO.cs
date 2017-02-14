namespace MyMoods.Domain.DTO
{
    public class MaslowCounterDTO
    {
        public MaslowCounterDTO(TagType type, long count, double totalPoints)
        {
            Area = type;
            Count = count;
            Points = new PointsDTO(this, totalPoints);
        }

        public TagType Area { get; private set; }
        public long Count { get; set; }
        public PointsDTO Points { get; set; }

        public class PointsDTO
        {
            public PointsDTO(MaslowCounterDTO counter, double total)
            {
                Counter = counter;
                Total = total;
            }

            protected MaslowCounterDTO Counter { get; set; }

            public double Total { get; set; }

            public double Avg => Counter.Count > 0 ? (Total / Counter.Count) : 5;
        }
    }
}
