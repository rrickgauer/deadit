using System.Text.Json.Serialization;

namespace Deadit.Lib.Domain.Errors;

public class ServiceDataResponse<T> : ServiceResponse
{
    public T? Data { get; set; }

    [JsonIgnore]
    public bool HasData => Data != null;
}
