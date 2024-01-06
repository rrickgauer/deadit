using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Model;
using System.Text.Json.Serialization;

namespace Deadit.Lib.Domain.TableView;

public class ViewUser : ITableView<ViewUser, User>
{
    [SqlColumn("id")]
    [CopyToPropertyAttribute<User>(nameof(User.Id))]
    public uint? Id { get; set; }

    [SqlColumn("email")]
    [CopyToPropertyAttribute<User>(nameof(User.Email))]
    public string? Email { get; set; }

    [SqlColumn("username")]
    [CopyToPropertyAttribute<User>(nameof(User.Username))]
    public string? Username { get; set; }

    [JsonIgnore]
    [SqlColumn("password")]
    [CopyToPropertyAttribute<User>(nameof(User.Password))]
    public string? Password { get; set; }

    [JsonIgnore]
    [SqlColumn("created_on")]
    [CopyToPropertyAttribute<User>(nameof(User.CreatedOn))]
    public DateTime CreatedOn { get; set; } = DateTime.Now;


    // ITableView
    public static explicit operator User(ViewUser other)
    {
        return ((ITableView<ViewUser, User>)other).CastToModel();
    }
}
