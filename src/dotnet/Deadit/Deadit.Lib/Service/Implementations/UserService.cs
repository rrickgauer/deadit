﻿using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Service.Contracts;

namespace Deadit.Lib.Service.Implementations;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.Always, InterfaceType = typeof(IUserService))]
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

    public async Task<ViewUser?> GetUserAsync(uint userId)
    {
        var dataRow = await _userRepository.SelectUserAsync(userId);

        if (dataRow != null)
        {
            return _tableMapperService.ToModel<ViewUser>(dataRow);
        }

        return null;
    }

    public async Task<IEnumerable<ViewUser>> GetMatchingUsersAsync(string email, string username)
    {
        var dataTable = await _userRepository.SelectMatchingUsersAsync(email, username);

        var users = _tableMapperService.ToModels<ViewUser>(dataTable);

        return users;   
    }

    public async Task<uint?> CreateUserAsync(SignupRequestForm signupForm)
    {
        var userId = await _userRepository.InsertAsync(signupForm);
        return userId;
    }
}
