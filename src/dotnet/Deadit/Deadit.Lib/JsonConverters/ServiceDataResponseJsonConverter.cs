using Deadit.Lib.Domain.Response;
using Deadit.Lib.Service.Implementations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Deadit.Lib.JsonConverters;

public class ServiceDataResponseJsonConverter<T> : JsonConverter<ServiceDataResponse<T>>
{
    public override ServiceDataResponse<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, ServiceDataResponse<T> value, JsonSerializerOptions options)
    {
        ApiResponse<T> apiResponse = new()
        {
            Data = value.Data,
            Errors = ErrorMessageService.ToErrorMessages(value.Errors),
        };

        JsonSerializer.Serialize(writer, apiResponse, options);
    }
}








