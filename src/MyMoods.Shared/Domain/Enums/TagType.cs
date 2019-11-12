using System.ComponentModel;

namespace MyMoods.Shared.Domain.Enums
{
    public enum TagType
    {
        [Description("Indefinido")]
        undefined = 0,

        [Description("Realização")]
        realization,

        [Description("Autoestima")]
        esteem,

        [Description("Social")]
        social,

        [Description("Segurança")]
        safety,

        [Description("Fisiológico")]
        physiological
    }
}
