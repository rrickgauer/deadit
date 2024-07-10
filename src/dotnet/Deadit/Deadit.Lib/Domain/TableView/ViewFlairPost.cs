using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Model;
using System.Text.Json.Serialization;

namespace Deadit.Lib.Domain.TableView;

public class ViewFlairPost : ViewCommunity, ITableView<ViewFlairPost, FlairPost>
{
    [SqlColumn("flair_post_id")]
    [CopyToProperty<FlairPost>(nameof(FlairPost.Id))]
    public uint? FlairPostId { get; set; }

    [SqlColumn("flair_post_community_id")]
    [CopyToProperty<FlairPost>(nameof(FlairPost.CommunityId))]
    public uint? FlairPostCommunityId { get; set; }

    [SqlColumn("flair_post_name")]
    [CopyToProperty<FlairPost>(nameof(FlairPost.Name))]
    public string? FlairPostName { get; set; }

    [SqlColumn("flair_post_color")]
    [CopyToProperty<FlairPost>(nameof(FlairPost.Color))]
    public string? FlairPostColor { get; set; }

    [SqlColumn("flair_post_created_on")]
    [CopyToProperty<FlairPost>(nameof(FlairPost.CreatedOn))]
    public DateTime FlairPostCreatedOn { get; set; } = DateTime.UtcNow;


    public static explicit operator FlairPost(ViewFlairPost other) => other.CastToModel<ViewFlairPost, FlairPost>();
}

