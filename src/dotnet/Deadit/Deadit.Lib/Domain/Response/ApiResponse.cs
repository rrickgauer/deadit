using Deadit.Lib.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadit.Lib.Domain.Response;


public class ApiResponseObject
{
    public IEnumerable<object> Errors { get; set; } = Enumerable.Empty<object>();
    public object? Data { get; set; }
}

public interface IApiResponse
{
    public ApiResponseObject ToApiResponseObject();
}




public class ApiResponse<T> : IApiResponse
{
    public List<ErrorMessage> Errors { get; set; } = new();
    public T? Data { get; set; }

    //public Dictionary<string,>


    public ApiResponseObject ToApiResponseObject()
    {
        return new()
        {
            Errors = Errors.Cast<object>(),
            Data = Data,
        };
    }

}
