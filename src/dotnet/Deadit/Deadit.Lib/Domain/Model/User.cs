namespace Deadit.Lib.Domain.Model;

using System;
using System.Text.Json.Serialization;

public class User
{
    public int? Id { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }

    [JsonIgnore]
    public string? Password { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
}

