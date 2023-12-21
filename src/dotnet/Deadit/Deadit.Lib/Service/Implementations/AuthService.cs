using Deadit.Lib.Auth;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.HttpResponses;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Service.Contracts;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Deadit.Lib.Service.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private ISession? Session => _httpContextAccessor?.HttpContext?.Session;


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="userService"></param>
    public AuthService(IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsClientLoggedIn()
    {
        if (Session != null)
        {
            SessionManager mgr = new(Session);
            return mgr.IsClientAuthorized();
        }

        return false;
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

    public async Task<ViewUser?> SignupUserAsync(SignupRequestForm signupForm)
    {

        var existingUsers = await _userService.GetMatchingUsersAsync(signupForm.Email, signupForm.Username);

        if (existingUsers.Any())
        {
            throw new HttpResponseException(HttpStatusCode.BadRequest, "The email or username is already taken");
        }


        var newUserId = await _userService.CreateUserAsync(signupForm);
        
        if (!newUserId.HasValue)
        {
            return null;
        }


        var newUser = await _userService.GetUserAsync(newUserId.Value);

        if (newUser != null)
        {
            SetClientSessionId(Session, newUserId.Value);
        }

        return newUser;
    }
}
