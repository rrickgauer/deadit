namespace Deadit.Lib.Domain.Model;

using System;

public class User
{
    public uint? Id { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
}

