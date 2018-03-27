using System.ComponentModel;

namespace MyMoods.Shared.Domain
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

    public enum QuestionType
    {
        [Description("Texto")]
        text = 1
    }

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

    public enum NotificationType
    {
        [Description("E-mail")]
        email = 1
    }

    public enum NotificationRecurrence
    {
        [Description("Diário")]
        daily = 1,

        [Description("Semanal")]
        weekly,

        [Description("Mensal")]
        monthly
    }
}
