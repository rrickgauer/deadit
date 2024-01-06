using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Microsoft.AspNetCore.Http;

namespace Deadit.Lib.Service.Contracts;

public interface IAuthService
{
    public Task<ServiceDataResponse<ViewUser>> LoginUserAsync(LoginRequestForm loginForm, ISession session);
    public Task<ServiceDataResponse<ViewUser>> SignupUserAsync(SignupRequestForm signupForm);
    public void ClearSessionData(ISession session);
    public bool IsClientLoggedIn();
}
