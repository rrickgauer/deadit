using Deadit.Lib.Domain.Forms;
using System.Data;

namespace Deadit.Lib.Repository.Contracts;

public interface IUserRepository
{
    public Task<DataRow?> SelectUserAsync(LoginRequestForm loginForm);
}
