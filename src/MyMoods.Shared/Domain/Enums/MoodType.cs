using System.ComponentModel;

namespace MyMoods.Shared.Domain.Enums
{
    public enum MoodType
    {
        [Description("Irritado")]
        angry = 1,

        [Description("Insatisfeito")]
        unsatisfied,

        [Description("Normal")]
        normal,

        [Description("Feliz")]
        happy,

        [Description("Apaixonado")]
        loving
    }
}
