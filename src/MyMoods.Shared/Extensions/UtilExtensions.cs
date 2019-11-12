using System;
using System.ComponentModel;
using System.Linq;

namespace MyMoods.Shared.Extensions
{
    public static class UtilExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var enumType = value.GetType();
            var field = enumType.GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false).ToArray();

            return attributes.Length == 0 ? value.ToString() : ((DescriptionAttribute)attributes[0]).Description;
        }
    }
}
