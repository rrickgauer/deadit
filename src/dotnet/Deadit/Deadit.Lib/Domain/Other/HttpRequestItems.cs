﻿using Deadit.Lib.Domain.Enum;
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

    public ViewFlairPost? FlairPost
    {
        get => GetValue<ViewFlairPost>(HttpRequestStorageKey.FlairPost);
        set => SetValue(HttpRequestStorageKey.FlairPost, value);
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
        if (_requestDict.ContainsKey(key))
        {
            _requestDict[key] = value;
        }
        else
        {
            _requestDict.Add(key, value);
        }
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

    public static void StoreFlairPost(IHttpContextAccessor contextAccessor, ViewFlairPost? flairPost)
    {
        if (contextAccessor.HttpContext?.Request.HttpContext is not HttpContext context)
        {
            return;
        }

        HttpRequestItems items = new(context)
        {
            FlairPost = flairPost,
        };
    }

}
