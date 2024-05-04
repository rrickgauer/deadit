using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Model;

namespace Deadit.Lib.Domain.TableView;

public class ViewCommunity : ITableView<ViewCommunity, Community>
{
    [SqlColumn("community_id")]
    [CopyToPropertyAttribute<Community>(nameof(Community.Id))]
    public uint? CommunityId { get; set; }

    [SqlColumn("community_name")]
    [CopyToPropertyAttribute<Community>(nameof(Community.Name))]
    public string? CommunityName { get; set; }

    [SqlColumn("community_title")]
    [CopyToPropertyAttribute<Community>(nameof(Community.Title))]
    public string? CommunityTitle { get; set; }

    [SqlColumn("community_owner_id")]
    [CopyToPropertyAttribute<Community>(nameof(Community.OwnerId))]
    public uint? CommunityOwnerId { get; set; }

    [SqlColumn("community_description")]
    [CopyToPropertyAttribute<Community>(nameof(Community.Description))]
    public string? CommunityDescription { get; set; }

    [SqlColumn("community_created_on")]
    [CopyToPropertyAttribute<Community>(nameof(Community.CreatedOn))]
    public DateTime CommunityCreatedOn { get; set; } = DateTime.Now;

    [SqlColumn("community_count_members")]
    public long CommunityCountMembers { get; set; } = 0;

    public string CommunityUrlGui => $"c/{CommunityName}";

    // ITableView
    public static explicit operator Community(ViewCommunity other)
    {
        return other.CastToModel();
    }
}
