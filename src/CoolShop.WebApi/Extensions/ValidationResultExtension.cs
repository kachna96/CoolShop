using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace CoolShop.WebApi.Extensions;

public static class ValidationResultExtension
{
    public static Dictionary<string, string[]> GetValidationErrors(this ValidationResult validationResult)
    {
        if (validationResult is null)
        {
            return new();
        }

        return validationResult
            .Errors
            .GroupBy(x => x.ErrorMessage, x => x.PropertyName)
            .ToDictionary(x => x.Key, group => group.ToArray());
    }
}
