using Deadit.Lib.Auth;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Service.Contracts;
using Microsoft.AspNetCore.Http;

namespace Deadit.Lib.Service.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="userService"></param>
    public AuthService(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Log the user in
    /// </summary>
    /// <param name="loginForm"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    public async Task<ViewUser?> LoginUserAsync(LoginRequestForm loginForm, ISession session)
    {
        // clear the current session data
        ClearSessionData(session);

        // validate the login credentials 
        var user = await _userService.GetUserAsync(loginForm);

        if (user == null)
        {
            return null;
        }

        // store the client's ID in session
        SetClientSessionId(session, user.Id);

        return user;
    }

    /// <summary>
    /// Clear the current session data
    /// </summary>
    /// <param name="session"></param>
    public void ClearSessionData(ISession session)
    {
        SetClientSessionId(session, null);
    }

    /// <summary>
    /// Set the client's session id
    /// </summary>
    /// <param name="session"></param>
    /// <param name="id"></param>
    private void SetClientSessionId(ISession session, int? id) 
    {
        SessionManager sessionManager = new(session)
        {
            ClientId = id
        };
    }
}
