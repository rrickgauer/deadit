using Deadit.Lib.Domain.Model;

namespace Deadit.Lib.Domain.Response;

public class ApiResponse<T>
{
    public List<ErrorMessage> Errors { get; set; } = new();
    public T? Data { get; set; }
}
