using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Deadit.Lib.Domain.Model;

public class ValidationFailureErrorMessage : ErrorMessage
{
    private readonly ModelStateDictionary _errorsDict = new();

    public Dictionary<string, string?> Errors => SetDict(_errorsDict);

    public ValidationFailureErrorMessage(ModelStateDictionary errors) : base()
    {
        Id = 6;
        Message = "Validation failed";
        _errorsDict = errors;

        SetDict(_errorsDict);
    }

    private Dictionary<string, string?> SetDict(ModelStateDictionary dict)
    {
        Dictionary<string, string?> result = new();

        foreach (var key in dict.Keys)
        {
            var dictValue = dict[key];
            var errorMessage = dictValue?.Errors.FirstOrDefault()?.ErrorMessage;
            result.Add(key, errorMessage);
        }

        return result;
    }
}
