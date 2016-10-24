using System.ComponentModel;

namespace MyMoods.Domain
{
    public enum MoodType
    {
        [Description("Triste")]
        sad = 1,

        [Description("Insatisfeito")]
        unsatisfied,

        [Description("Normal")]
        normal,

        [Description("Feliz")]
        happy,

        [Description("Apaixonado")]
        loving
    }

    public enum QuestionType
    {
        text = 1
    }

    public enum TagType
    {
        realization = 1,
        esteem,
        social,
        safety,
        physiological
    }
}
