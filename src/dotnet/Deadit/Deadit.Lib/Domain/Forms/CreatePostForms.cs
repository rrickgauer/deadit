using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Deadit.Lib.Domain.Forms;


public class CreatePostForms
{
    public abstract class CreatePostForm
    {
        public abstract string Title { get; set; }
    }

    public class CreateTextPostForm : CreatePostForm
    {
        [BindRequired]
        [StringLength(250)]
        public override required string Title { get; set; }

        [BindRequired]
        public string? Content { get; set; }
    }

    public class CreateLinkPostForm : CreatePostForm
    {
        [BindRequired]
        [StringLength(250)]
        public override required string Title { get; set; }

        [BindRequired]
        public required string Url { get; set; }
    }
}



