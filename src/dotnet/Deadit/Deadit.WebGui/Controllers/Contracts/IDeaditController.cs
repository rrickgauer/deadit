using Deadit.Lib.Auth;

namespace Deadit.WebGui.Controllers.Contracts;

public interface IDeaditController
{
    public SessionManager SessionManager { get; }
    public uint? ClientId { get; }
}


