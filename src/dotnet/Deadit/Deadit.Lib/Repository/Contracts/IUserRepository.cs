using Deadit.Lib.Domain.Forms;
using System.Data;

namespace Deadit.Lib.Repository.Contracts;

public interface IUserRepository
{
    public Task<DataRow?> SelectUserAsync(LoginRequestForm loginForm);
    public Task<DataRow?> SelectUserAsync(int userId);
    public Task<DataTable> SelectMatchingUsersAsync(string email, string username);
    public Task<int?> InsertAsync(SignupRequestForm signupForm);
}
