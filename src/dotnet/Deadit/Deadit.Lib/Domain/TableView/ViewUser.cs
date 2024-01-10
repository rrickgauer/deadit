using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Model;
using System.Text.Json.Serialization;

namespace Deadit.Lib.Domain.TableView;

public class ViewUser : ITableView<ViewUser, User>
{
    [SqlColumn("user_id")]
    [CopyToPropertyAttribute<User>(nameof(User.Id))]
    public uint? UserId { get; set; }

    [SqlColumn("user_email")]
    [CopyToPropertyAttribute<User>(nameof(User.Email))]
    public string? UserEmail { get; set; }

    [SqlColumn("user_username")]
    [CopyToPropertyAttribute<User>(nameof(User.Username))]
    public string? UserUsername { get; set; }

    [JsonIgnore]
    [SqlColumn("user_password")]
    [CopyToPropertyAttribute<User>(nameof(User.Password))]
    public string? UserPassword { get; set; }

    [JsonIgnore]
    [SqlColumn("user_created_on")]
    [CopyToPropertyAttribute<User>(nameof(User.CreatedOn))]
    public DateTime UserCreatedOn { get; set; } = DateTime.Now;


    // ITableView
    public static explicit operator User(ViewUser other)
    {
        return ((ITableView<ViewUser, User>)other).CastToModel();
    }
}
