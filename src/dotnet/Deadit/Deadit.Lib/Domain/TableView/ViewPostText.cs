using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Model;

namespace Deadit.Lib.Domain.TableView;

public class ViewPostText : ViewPost, 
    ITableView<ViewPostText, PostText>
{
    public override PostType? PostType => Enum.PostType.Text;

    [SqlColumn("post_content")]
    [CopyToProperty<PostText>(nameof(PostText.Content))]
    public string? PostContent { get; set; }


    #region - ITableView -

    public static explicit operator PostText(ViewPostText other) => other.CastToModel<ViewPostText, PostText>();

    #endregion
}





