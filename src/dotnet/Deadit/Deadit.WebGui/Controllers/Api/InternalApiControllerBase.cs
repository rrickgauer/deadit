using Deadit.Lib.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Api;


public abstract class InternalApiControllerBase : ControllerBase
{
    protected SessionManager SessionManager => new(Request.HttpContext.Session);
    protected uint? ClientId => SessionManager.ClientId;
}
