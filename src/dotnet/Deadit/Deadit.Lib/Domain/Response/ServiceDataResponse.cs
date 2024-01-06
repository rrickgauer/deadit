using System.Text.Json.Serialization;

namespace Deadit.Lib.Domain.Response;

public class ServiceDataResponse<T> : ServiceResponse
{
    public T? Data { get; set; }

    [JsonIgnore]
    public bool HasData => Data != null;


    public ServiceDataResponse() : base() { }

    public ServiceDataResponse(T? data) : base()
    {
        Data = data;
    }
}
