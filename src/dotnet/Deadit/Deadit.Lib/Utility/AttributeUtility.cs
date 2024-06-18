using System.Reflection;

namespace Deadit.Lib.Utility;

public static class AttributeUtility
{
    public static TAttr GetEnumAttribute<TEnum, TAttr>(TEnum value) where TEnum : struct, Enum where TAttr : Attribute
    {
        var enumName = Enum.GetName(value);

        var attr = typeof(TEnum).GetField(enumName!)!.GetCustomAttribute<TAttr>();

        if (attr is not TAttr validAttr)
        {
            throw new NotImplementedException();
        }

        return validAttr;
    }
}
