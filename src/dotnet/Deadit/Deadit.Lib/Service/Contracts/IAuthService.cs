using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.TableView;
using Microsoft.AspNetCore.Http;

namespace Deadit.Lib.Service.Contracts;

public interface IAuthService
{
    public Task<ViewUser?> LoginUserAsync(LoginRequestForm loginForm, ISession session);
    public void ClearSessionData(ISession session);
}
