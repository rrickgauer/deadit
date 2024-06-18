using Deadit.Lib.Domain.Attributes;
using System.Reflection;

namespace Deadit.Lib.Utility;

public static class EnumUtility
{
    public static string GetDropdownDisplay<TEnum>(TEnum enumValue) where TEnum : struct, Enum
    {
        var name = Enum.GetName(enumValue)!;

        if (typeof(TEnum).GetField(name)!.GetCustomAttribute<EnumDropdownDisplayAttribute>() is EnumDropdownDisplayAttribute attr)
        {
            return attr.DisplayText;
        }

        return name;
    }
}
