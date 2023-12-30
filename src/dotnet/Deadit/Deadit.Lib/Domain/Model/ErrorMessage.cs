using Deadit.Lib.Domain.Attributes;
using System.Collections;

namespace Deadit.Lib.Domain.Model;

public class ErrorMessage
{
    [SqlColumn("id")]
    public int? Id { get; set; }

    [SqlColumn("message")]
    public string? Message { get; set; }
}
