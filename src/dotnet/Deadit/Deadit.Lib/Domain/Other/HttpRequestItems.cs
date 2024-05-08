using Deadit.Lib.Domain.Enum;
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


    private T? GetValue<T>(HttpRequestStorageKey key)
    {
        if (!_requestDict.TryGetValue(HttpRequestStorageKey.CommunityId, out var value))
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


}
