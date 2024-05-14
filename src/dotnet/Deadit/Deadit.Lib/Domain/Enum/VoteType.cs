using System.Text.Json.Serialization;

namespace Deadit.Lib.Domain.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum VoteType : ushort
{
    Novote = 1,
    Downvote = 2,
    Upvote = 3,
    
}
