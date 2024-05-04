using System.Text.Json.Serialization;

namespace Deadit.Lib.Domain.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PostType : ushort
{
    Text = 1,
    Link = 2,
}

