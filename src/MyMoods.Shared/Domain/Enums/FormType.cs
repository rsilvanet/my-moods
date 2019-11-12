using System.ComponentModel;

namespace MyMoods.Shared.Domain.Enums
{
    public enum FormType
    {
        [Description("Simples")]
        simple = 1,

        [Description("Com tags padrão)")]
        general,

        [Description("Com tags padrão e customizadas")]
        generalWithCustomTags,

        [Description("Apenas com tags customizadas")]
        generalOnlyCustomTags
    }
}
