using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Deadit.Lib.Domain.Forms;

public class UpdateCommunityRequestForm
{
    [BindRequired]
    [StringLength(CommunityConstants.MaxTitleLength)]
    public required string Title { get; set; }

    [StringLength(CommunityConstants.MaxDescriptionLength)]
    public string? Description { get; set; }

    [BindRequired]
    public required CommunityType CommunityType { get; set; }

    [BindRequired]
    public required TextPostBodyRule TextPostBodyRule { get; set; }

    [BindRequired]
    public required bool AcceptingNewMembers { get; set; }

    [BindRequired]
    public required FlairPostRule FlairPostRule { get; set; }

    public void SetCommunityData(Community community)
    {
        community.Title = Title;
        community.Description = Description;
        community.CommunityType = CommunityType;
        community.TextPostBodyRule = TextPostBodyRule;
        community.FlairPostRule = FlairPostRule;
        community.MembershipClosedOn = AcceptingNewMembers ? null : DateTime.UtcNow;
    }
}
