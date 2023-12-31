﻿using Deadit.Lib.Domain.Enum;
using System.Text.Json.Serialization;

namespace Deadit.Lib.Domain.Response;

public class ServiceResponse
{
    public List<ErrorCode> Errors { get; set; } = new();

    [JsonIgnore]
    public bool Successful => Errors.Count == 0;

    public ServiceResponse() { }

    public ServiceResponse(ErrorCode errorCode)
    {
        Errors.Add(errorCode);
    }

    public ServiceResponse(IEnumerable<ErrorCode> errors)
    {
        Errors = errors.ToList();
    }


    public void Add(ErrorCode errorCode)
    {
        Errors.Add(errorCode);
    }


    public void Add(IEnumerable<ErrorCode> errors)
    {
        Errors.AddRange(errors);
    }


    public bool Exists(ErrorCode errorCode)
    {
        int matchingRecords = Errors.Where(e => e == errorCode).Count();

        return matchingRecords > 0;
    }
}
