using Deadit.Lib.Domain.Attributes;

namespace Deadit.Lib.Domain.Model;

public class ErrorMessage
{
    [SqlColumn("id")]
    public int? Id { get; set; }

    [SqlColumn("message")]
    public string? Message { get; set; }
}
