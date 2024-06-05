using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.TableView;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.Lib.Domain.Other;

public class HttpRequestItems(IDictionary<object, object?> requestDict)
{
    private readonly IDictionary<object, object?> _requestDict = requestDict;

    public HttpRequestItems(ActionContext context) : this(context.HttpContext.Items) { }
    public HttpRequestItems(HttpContext context) : this(context.Items) { }


    public uint? CommunityId
    {
        get => GetValue<uint?>(HttpRequestStorageKey.CommunityId);
        set => SetValue(HttpRequestStorageKey.CommunityId, value);
    }

    public ViewPost? Post
    {
        get => GetValue<ViewPost>(HttpRequestStorageKey.Post);
        set => SetValue(HttpRequestStorageKey.Post, value);
    }


    private T? GetValue<T>(HttpRequestStorageKey key)
    {
        if (!_requestDict.TryGetValue(key, out var value))
        {
            return default;
        }

        if (value is T typedValue)
        {
            return typedValue;
        }

        return default;
    }

    private void SetValue(HttpRequestStorageKey key, object? value) 
    {
        _requestDict.Add(key, value);
    }
    

    public static void StorePost(HttpContext? context, ViewPost? post)
    {
        if (context == null)
        {
            return;
        }

        HttpRequestItems items = new(context)
        {
            Post = post,
        };
    }

    public static void StoreCommunityId(HttpContext? context, uint? communityId)
    {
        if (context == null)
        {
            return;
        }

        HttpRequestItems items = new(context)
        {
            CommunityId = communityId,
        };
    }

}
