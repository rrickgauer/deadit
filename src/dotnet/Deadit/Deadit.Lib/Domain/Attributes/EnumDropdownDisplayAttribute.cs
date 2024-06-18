using System.Runtime.CompilerServices;

namespace Deadit.Lib.Domain.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class EnumDropdownDisplayAttribute(string displayText, [CallerMemberName] string fieldName = "") : Attribute
{
    public string DisplayText { get; } = displayText;
    public string FieldName { get; } = fieldName;
}
