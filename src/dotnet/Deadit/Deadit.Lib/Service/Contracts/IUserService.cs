using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.TableView;

namespace Deadit.Lib.Service.Contracts;

public interface IUserService
{
    public Task<ViewUser?> GetUserAsync(LoginRequestForm loginForm);
    public Task<ViewUser?> GetUserAsync(uint userId);
    public Task<List<ViewUser>> GetMatchingUsersAsync(string email, string username);
    public Task<uint?> CreateUserAsync(SignupRequestForm signupForm);
}
