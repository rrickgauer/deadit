using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Deadit.Lib.Domain.Forms;

public class CreateCommunityRequestForm
{
    public const int MaxCommunityNameLength        = 50;
    public const int MaxCommunitTitleLength        = 100;
    public const int MaxCommunityDescriptionLength = 200;

    [BindRequired]
    [StringLength(MaxCommunityNameLength)]
    [MinLength(3)]
    public required string Name { get; set; }

    [BindRequired]
    [StringLength(MaxCommunitTitleLength)]
    public required string Title { get; set; }

    [StringLength(MaxCommunityDescriptionLength)]
    public string? Description { get; set; }
}
