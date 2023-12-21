namespace Deadit.Lib.Domain.Errors;

public class ServiceDataResponse<T> : ServiceResponse
{
    public T? Data { get; set; }
    public bool HasData => Data != null;


 
}
