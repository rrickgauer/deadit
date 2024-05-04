using System.Runtime.CompilerServices;
using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class ErrorCodeGroupAttribute(ErrorCodeGroup errorCodeGroup, [CallerMemberName] string errorCodeName = "") : Attribute
{
    public ErrorCodeGroup ErrorCodeGroup { get; } = errorCodeGroup;
    public string ErrorCodeName { get; } = errorCodeName;
}