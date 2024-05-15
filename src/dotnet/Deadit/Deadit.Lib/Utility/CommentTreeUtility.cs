/**************************************************************************************************************

Given a flat list of comments, build a nested tree that returns a list of the root comments with their replies.

Copied from: https://codereview.stackexchange.com/a/163985
 
**************************************************************************************************************/
using Deadit.Lib.Domain.TableView;

namespace Deadit.Lib.Utility;

public static class CommentTreeUtility
{
    public static List<ViewComment> ToTree(this List<ViewComment> nodes)
    {
        ViewComment root = new();

        root.BuildTree(nodes);

        return root.CommentReplies;
    }

    private static ViewComment BuildTree(this ViewComment root, List<ViewComment> nodes)
    {
        if (nodes.Count == 0) { return root; }

        var children = root.FetchChildren(nodes).ToList();
        root.CommentReplies.AddRange(children);
        root.RemoveChildren(nodes);

        for (int i = 0; i < children.Count; i++)
        {
            children[i] = children[i].BuildTree(nodes);
            
            if (nodes.Count == 0) 
            { 
                break; 
            }
        }

        return root;
    }

    public static IEnumerable<ViewComment> FetchChildren(this ViewComment root, List<ViewComment> nodes)
    {
        return nodes.Where(n => n.CommentParentId == root.CommentId);
    }

    public static void RemoveChildren(this ViewComment root, List<ViewComment> nodes)
    {
        foreach (var node in root.CommentReplies)
        {
            nodes.Remove(node);
        }
    }


}
