using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Model;

public class DeaditErrorMessage
{
    public DeaditErrorId? Id { get; set; }
    public string? Message { get; set; }
}
