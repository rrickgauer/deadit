using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Deadit.Lib.Domain.Forms;

public class FlairPostForm
{
    [BindRequired]
    public required string CommunityName { get; set; }

    [BindRequired]
    public required string Name { get; set; }

    [BindRequired]
    public required string Color { get; set; }
}
