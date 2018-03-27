using System;
using System.ComponentModel;
using System.Linq;

namespace MyMoods.Shared.Util
{
    public static class UtilEnums
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
