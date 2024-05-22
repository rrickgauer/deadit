using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Deadit.Lib.Domain.Forms;

public class EditPostForm
{
    [BindRequired]
    public required string Content { get; set; }
}



