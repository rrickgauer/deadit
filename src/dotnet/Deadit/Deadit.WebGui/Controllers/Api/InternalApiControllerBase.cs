using Deadit.Lib.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Api;


public class InternalApiControllerBase : ControllerBase
{
    protected SessionManager SessionManager => new(Request.HttpContext.Session);
    protected uint ClientId => SessionManager.ClientId.Value;
}
