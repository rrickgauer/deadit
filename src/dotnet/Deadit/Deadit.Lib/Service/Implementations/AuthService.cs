using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Service.Contracts;
using Microsoft.AspNetCore.Http;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Other;

namespace Deadit.Lib.Service.Implementations;

[AutoInject<IAuthService>(AutoInjectionType.Scoped, InjectionProject.Always)]
public class AuthService(IUserService userService, IHttpContextAccessor httpContextAccessor) : IAuthService
{
    private const int MinPasswordLength = 8;

    private readonly IUserService _userService = userService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private ISession? Session => _httpContextAccessor?.HttpContext?.Session;

    public bool IsClientLoggedIn()
    {
        if (Session != null)
        {
            SessionManager mgr = new(Session);
            return mgr.IsClientAuthorized();
        }

        return false;
    }

    public async Task<ServiceDataResponse<ViewUser>> LoginUserAsync(LoginRequestForm loginForm, ISession session)
    {
        // clear the current session data
        ClearSessionData(session);

        ServiceDataResponse<ViewUser> result = new();

        try
        {
            result.Data = await _userService.GetUserAsync(loginForm);
        }
        catch(RepositoryException ex)
        {
            return new(ex);
        }


        if (result.Data != null)
        {
            // store the client's ID in session
            SetClientSessionId(session, result.Data.UserId);
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
    private void SetClientSessionId(ISession session, uint? id) 
    {
        SessionManager sessionManager = new(session)
        {
            ClientId = id
        };
    }

    public async Task<ServiceDataResponse<ViewUser>> SignupUserAsync(SignupRequestForm signupForm)
    {

        try
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
                ArgumentNullException.ThrowIfNull(Session);
                SetClientSessionId(Session, newUserId.Value);
            }

            return response;
        }
        catch (RepositoryException ex)
        {
            return ex;
        }
    }

    private async Task<ServiceDataResponse<ViewUser>> ValidateNewUserAccountAsync(SignupRequestForm signupForm)
    {
        try
        {
            ServiceDataResponse<ViewUser> response = new();

            if (signupForm.Password.Length < MinPasswordLength)
            {
                response.AddError(ErrorCode.SignupInvalidPassword);
            }

            var existingUsers = await _userService.GetMatchingUsersAsync(signupForm.Email, signupForm.Username);

            if (existingUsers.Any())
            {
                var errors = GetSignupErrors(existingUsers, signupForm);
                response.AddError(errors);
            }

            return response;
        }
        catch(RepositoryException ex)
        {
            return ex;
        }

    }

    private List<ErrorCode> GetSignupErrors(IEnumerable<ViewUser> existingUsers, SignupRequestForm signupForm)
    {
        List<ErrorCode> errors = new();        

        foreach(var user in existingUsers)
        {
            if (user.UserEmail == signupForm.Email)
            {
                errors.Add(ErrorCode.SignUpEmailTaken);
            }
            
            if (user.UserUsername == signupForm.Username)
            {
                errors.Add(ErrorCode.SignupUsernameTaken);
            }
        }

        return errors;
    }

}
