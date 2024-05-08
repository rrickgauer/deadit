using Deadit.Lib.Domain.Other;
using Deadit.Lib.Domain.Response;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Contracts;


public abstract class InternalApiController : ControllerBase, IDeaditController
{
    public SessionManager SessionManager => new(Request.HttpContext.Session);
    public uint? ClientId => SessionManager.ClientId;

    public HttpRequestItems RequestItems => new(HttpContext.Items);



    protected IActionResult ReturnStandardDataResponse<T>(ServiceDataResponse<T> response)
    {
        if (!response.Successful)
        {
            return BadRequest(response);
        }

        return Ok(response);
    
    }
}


