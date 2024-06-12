using Deadit.Lib.Domain.Constants;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Deadit.Lib.Domain.Forms;

public class CreateCommunityRequestForm
{ 


    [BindRequired]
    [StringLength(CommunityConstants.MaxNameLength)]
    [MinLength(CommunityConstants.MinimumNameLength)]
    public virtual required string Name { get; set; }

    [BindRequired]
    [StringLength(CommunityConstants.MaxTitleLength)]
    public required string Title { get; set; }

    [StringLength(CommunityConstants.MaxDescriptionLength)]
    public string? Description { get; set; }
}
