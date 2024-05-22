using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Model;

namespace Deadit.Lib.Domain.TableView;

public class ViewPostLink : ViewPost, ITableView<ViewPostLink, PostLink>
{
    public override PostType? PostType => Enum.PostType.Link;

    [SqlColumn("post_url")]
    [CopyToProperty<PostLink>(nameof(PostLink.Url))]
    public string? PostUrl { get; set; }


    public override string PostBodyContent => PostUrl ?? string.Empty;


    public override void HandlePostDeleted()
    {
        base.HandlePostDeleted();

        if (PostDeletedOn.HasValue)
        {   
            PostUrl = null;
        }
    }

    #region - ITableView -

    public static explicit operator PostLink(ViewPostLink other) => other.CastToModel<ViewPostLink, PostLink>();

    #endregion
}





