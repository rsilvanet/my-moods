using System.ComponentModel;

namespace MyMoods.Shared.Domain.Enums
{
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
