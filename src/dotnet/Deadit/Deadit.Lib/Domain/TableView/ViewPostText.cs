using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Utility;
using System.Text.Json.Serialization;

namespace Deadit.Lib.Domain.TableView;

public class ViewPostText : ViewPost, 
    ITableView<ViewPostText, PostText>
{
    public override PostType? PostType => Enum.PostType.Text;

    [SqlColumn("post_content")]
    [CopyToProperty<PostText>(nameof(PostText.Content))]
    public string? PostContent { get; set; }


    [JsonIgnore]
    public override string? PostBodyContent => PostContent;


    public override void HandlePostDeleted()
    {
        base.HandlePostDeleted();

        if (PostIsRemoved)
        {
            PostContent = RemovedContentText;
        }
        else if (PostDeletedOn.HasValue)
        {
            PostContent = DeletedContentText;
        }
    }



    #region - ITableView -

    public static explicit operator PostText(ViewPostText other) => other.CastToModel<ViewPostText, PostText>();

    #endregion
}





