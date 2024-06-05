using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Deadit.Lib.Domain.Forms;

public class CommentForm
{
    [BindRequired]
    public required Guid PostId { get; set; }

    [BindRequired]
    public required string Content { get; set; }

    [BindRequired]
    public required Guid? ParentId { get; set; }
}
