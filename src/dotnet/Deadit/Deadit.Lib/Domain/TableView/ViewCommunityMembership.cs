using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Model;

namespace Deadit.Lib.Domain.TableView;

public class ViewCommunityMembership :
    ITableView<ViewCommunityMembership, CommunityMembership>,
    ITableView<ViewCommunityMembership, ViewUser>,
    ITableView<ViewCommunityMembership, ViewCommunity>,
    ITableView<ViewCommunityMembership, Community>
{

    #region - Community -

    [SqlColumn("community_id")]
    [CopyToPropertyAttribute<ViewCommunity>(nameof(ViewCommunity.Id))]
    [CopyToPropertyAttribute<CommunityMembership>(nameof(CommunityMembership.CommunityId))]
    public uint? CommunityId { get; set; }

    [SqlColumn("community_name")]
    [CopyToPropertyAttribute<ViewCommunity>(nameof(ViewCommunity.Name))]
    public string? CommunityName { get; set; }

    [SqlColumn("community_title")]
    [CopyToPropertyAttribute<ViewCommunity>(nameof(ViewCommunity.Title))]
    public string? CommunityTitle { get; set; }

    [SqlColumn("community_description")]
    [CopyToPropertyAttribute<ViewCommunity>(nameof(ViewCommunity.Description))]
    public string? CommunityDescription { get; set; }

    [SqlColumn("community_owner_id")]
    [CopyToPropertyAttribute<ViewCommunity>(nameof(ViewCommunity.OwnerId))]
    public uint? CommunityOwnerId { get; set; }

    [SqlColumn("community_created_on")]
    [CopyToPropertyAttribute<ViewCommunity>(nameof(ViewCommunity.CreatedOn))]
    public DateTime CommunityCreatedOn { get; set; } = DateTime.Now;

    [SqlColumn("count_members")]
    [CopyToPropertyAttribute<ViewCommunity>(nameof(ViewCommunity.CountMembers))]
    public long CountMembers { get; set; } = 0;

    #endregion

    #region - User -

    [SqlColumn("user_id")]
    [CopyToPropertyAttribute<ViewUser>(nameof(ViewUser.Id))]
    [CopyToPropertyAttribute<CommunityMembership>(nameof(CommunityMembership.UserId))]
    public uint? UserId { get; set; }

    [SqlColumn("user_username")]
    [CopyToPropertyAttribute<ViewUser>(nameof(ViewUser.Username))]
    public string? UserUsername { get; set; }

    [SqlColumn("user_email")]
    [CopyToPropertyAttribute<ViewUser>(nameof(ViewUser.Email))]
    public string? UserEmail { get; set; }

    [SqlColumn("user_password")]
    [CopyToPropertyAttribute<ViewUser>(nameof(ViewUser.Password))]
    public string? UserPassword { get; set; }

    [SqlColumn("user_created_on")]
    [CopyToPropertyAttribute<ViewUser>(nameof(ViewUser.CreatedOn))]
    public DateTime UserCreatedOn { get; set; } = DateTime.Now;


    #endregion

    #region - Membership -

    [SqlColumn("joined_community_on")]
    [CopyToPropertyAttribute<CommunityMembership>(nameof(CommunityMembership.CreatedOn))]
    public DateTime JoinedOn { get; set; } = DateTime.Now;

    #endregion

    #region - ITableView -

    public static explicit operator Community(ViewCommunityMembership other)
    {
        return (Community)((ViewCommunity)other);
    }

    public static explicit operator ViewCommunity(ViewCommunityMembership other)
    {
        return ((ITableView<ViewCommunityMembership, ViewCommunity>)other).CastToModel();
    }

    public static explicit operator ViewUser(ViewCommunityMembership other)
    {
        return ((ITableView<ViewCommunityMembership, ViewUser>)other).CastToModel();
    }

    public static explicit operator CommunityMembership(ViewCommunityMembership other)
    {
        return ((ITableView<ViewCommunityMembership, CommunityMembership>)other).CastToModel();
    }
    
    #endregion
}
