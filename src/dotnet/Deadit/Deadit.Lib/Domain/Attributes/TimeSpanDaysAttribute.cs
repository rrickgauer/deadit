namespace Deadit.Lib.Domain.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class TimeSpanDaysAttribute(int days) : Attribute
{
    public int Days { get; } = days;
    public bool IsAllTime => Days == int.MinValue;
}
