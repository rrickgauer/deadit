using Deadit.Lib.Domain.Response;

namespace Deadit.Lib.Auth;


public interface IPermissionsAuth<T>
{
    public ServiceResponse HasPermission(T data);
}

public interface IAsyncPermissionsAuth<T>
{
    public Task<ServiceResponse> HasPermissionAsync(T data);
}
