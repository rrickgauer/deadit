using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Model;

namespace Deadit.Lib.Domain.TableView;

public class ViewCommunity : ITableView<ViewCommunity, Community>
{
    [SqlColumn("id")]
    [CopyToPropertyAttribute<Community>(nameof(Community.Id))]
    public uint? Id { get; set; }

    [SqlColumn("community_name")]
    [CopyToPropertyAttribute<Community>(nameof(Community.Name))]
    public string? Name { get; set; }

    [SqlColumn("community_title")]
    [CopyToPropertyAttribute<Community>(nameof(Community.Title))]
    public string? Title { get; set; }

    [SqlColumn("community_owner_id")]
    [CopyToPropertyAttribute<Community>(nameof(Community.OwnerId))]
    public int? OwnerId { get; set; }

    [SqlColumn("community_description")]
    [CopyToPropertyAttribute<Community>(nameof(Community.Description))]
    public string? Description { get; set; }

    [SqlColumn("community_created_on")]
    [CopyToPropertyAttribute<Community>(nameof(Community.CreatedOn))]
    public DateTime CreatedOn { get; set; } = DateTime.Now;

    [SqlColumn("count_members")]
    public long CountMembers { get; set; } = 0;


    public string UrlGui => $"communities/{Id}";

    // ITableView
    public static explicit operator Community(ViewCommunity other)
    {
        return ((ITableView<ViewCommunity, Community>)other).CastToModel();
    }
}
