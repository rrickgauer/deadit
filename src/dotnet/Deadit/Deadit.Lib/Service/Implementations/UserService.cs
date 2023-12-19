using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Service.Contracts;

namespace Deadit.Lib.Service.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITableMapperService _tableMapperService;

    public UserService(IUserRepository userRepository, ITableMapperService tableMapperService)
    {
        _userRepository = userRepository;
        _tableMapperService = tableMapperService;
    }

    public async Task<ViewUser?> GetUserAsync(LoginRequestForm loginForm)
    {
        var datarow = await _userRepository.SelectUserAsync(loginForm);

        if (datarow == null)
        {
            return null;
        }

        var user = _tableMapperService.ToModel<ViewUser>(datarow);

        return user;
    }
}
