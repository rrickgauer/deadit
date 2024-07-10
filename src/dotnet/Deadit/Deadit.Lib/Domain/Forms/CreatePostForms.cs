using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Deadit.Lib.Domain.Forms;

public class CreatePostForms
{
    public abstract class CreatePostForm
    {
        [BindRequired]
        [StringLength(250)]
        public required string Title { get; set; }

        [BindRequired]
        public required string CommunityName { get; set; }

        [BindRequired]
        public required uint? FlairPostId { get; set; }
    }

    public class CreateTextPostForm : CreatePostForm
    {
        [BindRequired]
        public string? Content { get; set; }
    }

    public class CreateLinkPostForm : CreatePostForm
    {
        [BindRequired]
        public required string Url { get; set; }
    }
}



