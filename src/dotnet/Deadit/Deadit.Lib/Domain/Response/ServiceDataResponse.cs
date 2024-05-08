using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using System.Text.Json.Serialization;

namespace Deadit.Lib.Domain.Response;


public class ServiceDataResponse<T> : ServiceResponse
{
    public T? Data { get; set; }

    [JsonIgnore]
    public bool HasData => Data != null;


    public ServiceDataResponse() : base() { }
    public ServiceDataResponse(IEnumerable<ErrorCode> errors) : base(errors) { }
    public ServiceDataResponse(ErrorCode errorCode) : base(errorCode) { }
    public ServiceDataResponse(ServiceResponse other) : base(other) { }
    public ServiceDataResponse(RepositoryException ex) : base(ex) { }


    public ServiceDataResponse(T? data) : base()
    {
        Data = data;
    }


    public ServiceDataResponse(ServiceDataResponse<T> other) : base(other)
    {
        Data = other.Data;
    }

    public static implicit operator ServiceDataResponse<T>(RepositoryException ex)
    {
        return new(ex);
    }
}
