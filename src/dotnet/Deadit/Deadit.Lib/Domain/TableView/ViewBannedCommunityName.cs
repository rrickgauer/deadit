using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Model;

namespace Deadit.Lib.Domain.TableView;

public class ViewBannedCommunityName : ITableView<ViewBannedCommunityName, BannedCommunityName>
{
    [SqlColumn("id")]
    [CopyToProperty<BannedCommunityName>(nameof(BannedCommunityName.Id))]
    public uint? Id { get; set; }

    [SqlColumn("name")]
    [CopyToProperty<BannedCommunityName>(nameof(BannedCommunityName.Name))]
    public string? Name { get; set; }


    // ITableView
    public static explicit operator BannedCommunityName(ViewBannedCommunityName other)
    {
        return ((ITableView<ViewBannedCommunityName, BannedCommunityName>)other).CastToModel();
    }
}
