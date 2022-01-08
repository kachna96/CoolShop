using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace CoolShop.WebApi.Extensions;

public static class ValidationResultsExtension
{
    public static Dictionary<string, string[]> GetValidationErrors(this ValidationResult validationResult)
    {
        if (validationResult is null)
        {
            return new Dictionary<string, string[]>();
        }

        return validationResult
            .Errors
            .GroupBy(x => x.ErrorMessage, x => x.PropertyName)
            .ToDictionary(x => x.Key, group => group.ToArray());
    }
}
