using Deadit.Lib.Auth;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Service.Contracts;
using Microsoft.AspNetCore.Http;
using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Service.Implementations;

public class AuthService : IAuthService
{
    private const int MinPasswordLength = 8;

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

    public async Task<ServiceDataResponse<ViewUser>> LoginUserAsync2(LoginRequestForm loginForm, ISession session)
    {
        // clear the current session data
        ClearSessionData(session);

        ServiceDataResponse<ViewUser> result = new()
        {
            Data = await _userService.GetUserAsync(loginForm)
        };

        if (result.Data != null)
        {
            // store the client's ID in session
            SetClientSessionId(session, result.Data.Id);
        }

        return result;
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

    public async Task<ServiceDataResponse<ViewUser>> SignupUserAsync(SignupRequestForm signupForm)
    {
        ServiceDataResponse<ViewUser> response = await ValidateNewUserAccountAsync(signupForm);

        if (!response.Successful)
        {
            return response;
        }

        var newUserId = await _userService.CreateUserAsync(signupForm);
        response.Data = await _userService.GetUserAsync(newUserId.Value);

        if (response.Data != null)
        {
            SetClientSessionId(Session, newUserId.Value);
        }

        return response;
    }

    private async Task<ServiceDataResponse<ViewUser>> ValidateNewUserAccountAsync(SignupRequestForm signupForm)
    {
        ServiceDataResponse<ViewUser> response = new();

        if (signupForm.Password.Length < MinPasswordLength)
        {
            response.Add(ErrorCode.SignupInvalidPassword);
        }

        var existingUsers = await _userService.GetMatchingUsersAsync(signupForm.Email, signupForm.Username);

        if (existingUsers.Any())
        {
            var errors = GetSignupErrors(existingUsers, signupForm);
            response.Add(errors);
        }

        return response;
    }

    private List<ErrorCode> GetSignupErrors(IEnumerable<ViewUser> existingUsers, SignupRequestForm signupForm)
    {
        List<ErrorCode> errors = new();        

        foreach(var user in existingUsers)
        {
            if (user.Email == signupForm.Email)
            {
                errors.Add(ErrorCode.SignUpEmailTaken);
            }
            
            if (user.Username == signupForm.Username)
            {
                errors.Add(ErrorCode.SignupUsernameTaken);
            }
        }

        return errors;
    }

}
