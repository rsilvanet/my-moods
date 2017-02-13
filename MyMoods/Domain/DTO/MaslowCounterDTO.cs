namespace MyMoods.Domain.DTO
{
    public class MaslowCounterDTO
    {
        public MaslowCounterDTO(TagType type, long count, double totalPoints)
        {
            Area = type;
            Count = count;
            Points = new PointsDTO(this);
        }

        public TagType Area { get; private set; }
        public long Count { get; set; }
        public PointsDTO Points { get; set; }

        public class PointsDTO
        {
            MaslowCounterDTO _counter;

            public PointsDTO(MaslowCounterDTO counter)
            {
                _counter = counter;
            }

            public double Total { get; set; }

            public double Avg => _counter.Count > 0 ? (Total / _counter.Count) : 0;
        }
    }
}
