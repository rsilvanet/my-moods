using MyMoods.Shared.Util;

namespace MyMoods.Shared.Domain.DTO
{
    public class MoodDTO
    {
        public MoodDTO(MoodType type, double points, string image, string tagsHelpText)
        {
            Value = type.ToString();
            Title = type.GetDescription();
            Points = points;
            Image = image;
            TagsHelpText = tagsHelpText;
        }

        public string Value { get; private set; }
        public string Title { get; private set; }
        public double Points { get; private set; }
        public string Image { get; private set; }
        public string TagsHelpText { get; set; }
    }
}
