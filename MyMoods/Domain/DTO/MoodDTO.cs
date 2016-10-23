using MyMoods.Util;

namespace MyMoods.Domain.DTO
{
    public class MoodDTO
    {
        public MoodDTO(MoodType type)
        {
            Value = type.ToString();
            Title = type.GetDescription();
            Points = (int)type;
            Image = "https://encrypted-tbn1.gstatic.com/images?q=tbn:ANd9GcTFjnuJAb5EMJFoEBGAgXSIKdpvDwHbUV78kBMOxXS-HzqTkWxdBhVoacc";
        }

        public string Value { get; private set; }
        public string Title { get; private set; }
        public int Points { get; private set; }
        public string Image { get; private set; }
    }
}
