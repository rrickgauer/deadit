using Deadit.Lib.Domain.Other;

namespace Deadit.WebGui.Controllers.Contracts;

public interface IDeaditController
{
    public SessionManager SessionManager { get; }
    public uint? ClientId { get; }

    public HttpRequestItems RequestItems { get; }
}


