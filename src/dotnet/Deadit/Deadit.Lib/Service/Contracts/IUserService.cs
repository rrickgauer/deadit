using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.TableView;

namespace Deadit.Lib.Service.Contracts;

public interface IUserService
{
    public Task<ViewUser?> GetUserAsync(LoginRequestForm loginForm);
}
