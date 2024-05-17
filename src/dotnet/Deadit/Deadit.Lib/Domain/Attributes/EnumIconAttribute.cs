namespace Deadit.Lib.Domain.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class EnumIconAttribute(string icon) : Attribute
{
    public string Icon { get; } = icon;
}




