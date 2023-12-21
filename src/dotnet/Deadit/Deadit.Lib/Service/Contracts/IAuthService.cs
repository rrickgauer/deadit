using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.TableView;
using Microsoft.AspNetCore.Http;

namespace Deadit.Lib.Service.Contracts;

public interface IAuthService
{
    public Task<ViewUser?> LoginUserAsync(LoginRequestForm loginForm, ISession session);
    public void ClearSessionData(ISession session);
    
    //public Task<ViewUser?> SignupUserAsync(SignupRequestForm signupForm);
    public Task<ServiceDataResponse<ViewUser>> SignupUserAsync(SignupRequestForm signupForm);

    public bool IsClientLoggedIn();
}
