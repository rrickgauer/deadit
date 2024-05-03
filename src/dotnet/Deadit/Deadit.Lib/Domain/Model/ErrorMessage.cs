using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Model;

public class ErrorMessage
{
    [SqlColumn("id")]
    public int? Id { get; set; }

    [SqlColumn("message")]
    public string? Message { get; set; }


    public static explicit operator ErrorCode(ErrorMessage message)
    {
        ArgumentNullException.ThrowIfNull(message.Id);
        return (ErrorCode)message.Id;
    }
}
