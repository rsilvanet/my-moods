using MyMoods.Util;

namespace MyMoods.Domain.DTO
{
    public class MoodDTO
    {
        public MoodDTO(MoodType type, string tagsHelpText)
        {
            Value = type.ToString();
            Title = type.GetDescription();
            Points = (int)type;
            Image = $"/assets/emojis/{Value}.png";
            TagsHelpText = tagsHelpText;
        }

        public string Value { get; private set; }
        public string Title { get; private set; }
        public int Points { get; private set; }
        public string Image { get; private set; }
        public string TagsHelpText { get; set; }
    }
}
