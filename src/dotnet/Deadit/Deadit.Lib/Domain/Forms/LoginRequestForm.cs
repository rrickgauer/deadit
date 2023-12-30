using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Deadit.Lib.Domain.Forms;

public class LoginRequestForm
{
    [BindRequired]
    public required string Username {  get; set; }

    [BindRequired]
    public required string Password { get; set; }
}
