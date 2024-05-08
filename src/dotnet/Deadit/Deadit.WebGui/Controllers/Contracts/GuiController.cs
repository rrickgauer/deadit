using Deadit.Lib.Domain.Other;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Contracts;

public class GuiController : Controller, IDeaditController
{
    public SessionManager SessionManager => new(Request.HttpContext.Session);
    public uint? ClientId => SessionManager.ClientId;
}


