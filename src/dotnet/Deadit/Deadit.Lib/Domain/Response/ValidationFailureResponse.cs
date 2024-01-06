using Deadit.Lib.Domain.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Deadit.Lib.Domain.Response;

public class ValidationFailureResponse
{
    public List<ValidationFailureErrorMessage> Errors { get; set; } = new();
    public object? Data { get; set; } = null;

    public ValidationFailureResponse(ModelStateDictionary modelStateDict)
    {
        Errors = new()
        {
            new(modelStateDict),
        };
    }
}
