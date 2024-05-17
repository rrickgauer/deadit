using Deadit.Lib.Domain.Response;

namespace Deadit.Lib.Service.Contracts;

public interface IVMService<in TArgs, TViewModel>
{
    public Task<ServiceDataResponse<TViewModel>> GetViewModelAsync(TArgs args);
}
