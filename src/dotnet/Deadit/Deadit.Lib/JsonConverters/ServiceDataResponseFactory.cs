using System.Text.Json;
using System.Text.Json.Serialization;
using Deadit.Lib.Domain.Response;

namespace Deadit.Lib.JsonConverters;

public class ServiceDataResponseFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (IsServiceDataResponse(typeToConvert))
        {
            return true;
        }

        if (IsServiceDataResponse(typeToConvert))
        {
            return true;
        }

        return false;
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        if (IsServiceDataResponse(typeToConvert))
        {
            return GetServiceDataResponseJsonConverter(typeToConvert);
        }

        throw new JsonException($"No converter for {typeToConvert}");
    }


    private static JsonConverter? GetServiceDataResponseJsonConverter(Type typeToConvert)
    {
        // Get the specific type parameter [T] of the ServiceDataResponse<T>
        Type typeArgument = typeToConvert.GetGenericArguments()[0];

        Type converterType = typeof(ServiceDataResponseJsonConverter<>)!.MakeGenericType(typeArgument);

        // Create a converter of type ServiceDataResponseConverter<T>
        JsonConverter? converter = (JsonConverter)Activator.CreateInstance(converterType)!;

        return converter;
    }


    private static bool IsServiceDataResponse(Type typeToConvert)
    {
        if (!typeToConvert.IsGenericType)
        {
            return false;
        }

        if (typeToConvert.GetGenericTypeDefinition() == typeof(ServiceDataResponse<>))
        {
            return true;
        }

        return false;
    }


    private static bool IsServiceResponse(Type typeToConvert)
    {
        if (typeToConvert == typeof(ServiceResponse))
        {
            return true;
        }

        return false;
    }
}








